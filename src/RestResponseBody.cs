//-------------------------------------------------------------------------------
// <copyright>
//    Copyright (c) 2012 Nripendra Nath Newa (nripendra@uba-solutions.com).
//    Licensed under the MIT License (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.opensource.org/licenses/mit-license.php
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Nripendra Nath Newa (nripendra@uba-solutions.com).</author>
//-------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Resty.Net
{
    using Extensions;

    public class RestResponseBody : IDisposable
    {
        private byte[] _contentBytes;
        private string _contentString;
        private MemoryStream _contentStream;

        private Stream _responseStream;
        private string _characterSet;

        public Exception Error { get; private set; }

        public RestResponseBody(Stream responseStream, string characterSet)
        {
            _responseStream = responseStream;
            _characterSet = characterSet;
        }

        public virtual string ReadAsString()
        {
            return ReadAsStringAsync().Result;
        }

        public virtual Task<string> ReadAsStringAsync()
        {
            if (_contentString == null)
            {
                return ReadAsByteArrayAsync().ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Error = t.Exception.Flatten();
                        _contentString = "";
                    }
                    else if (t.IsCanceled)
                    {
                        _contentString = "";
                    }
                    else
                        _contentString = GetEncoding().GetString(t.Result);

                    return _contentString;
                });
            }
            else
            {
                return Task.Factory.StartNew(() => _contentString);
            }
        }

        public virtual Stream ReadAsStream()
        {
            return ReadAsStreamAsync().Result;
        }

        public virtual Task<Stream> ReadAsStreamAsync()
        {
            if (_contentStream == null)
            {
                MemoryStream destinationStream = new MemoryStream();
                return CopyToAsync(destinationStream).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Error = t.Exception.Flatten();
                        return null;
                    }
                    else if (t.IsCanceled)
                    {
                        return null;
                    }
                    else
                        _contentStream = (MemoryStream)t.Result;

                    return (Stream)_contentStream;
                });
            }
            else
            {
                return Task.Factory.StartNew(() => (Stream)_contentStream);
            }
        }

        public virtual byte[] ReadAsByteArray()
        {
            return ReadAsByteArrayAsync().Result;
        }

        public virtual Task<byte[]> ReadAsByteArrayAsync()
        {
            if (_contentBytes == null)
            {
                MemoryStream destinationStream = new MemoryStream();
                return CopyToAsync(destinationStream).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Error = t.Exception.Flatten();
                        return null;
                    }
                    else if (t.IsCanceled)
                    {
                        return null;
                    }
                    else
                        _contentBytes = ((MemoryStream)t.Result).ToArray();

                    return _contentBytes;
                });
            }
            else
            {
                return Task.Factory.StartNew(() => _contentBytes);
            }
        }

        /// <summary>
        /// Copies the WebResponse's stream into ResponseStream property.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public virtual Task<Stream> CopyToAsync(Stream destination)
        {
            const int readSize = 256;
            byte[] buffer = new byte[readSize];
            int start = 0;
            return _responseStream.CopyToAsync(destination, buffer, start, readSize, (object)null).ContinueWith<int>(task =>
            {
                destination.Position = 0;
                _responseStream.Close();
                if (task.IsFaulted)
                {
                    Error = task.Exception.Flatten();
                    return 0;
                }
                else if (task.IsCanceled)
                {
                    return 0;
                }
                else
                    return task.Result;
            }).ContinueWith((t) => destination);
        }

        /// <summary>
        /// Get Encoding from the response character set.
        /// </summary>
        /// <returns>Instance of Encoding</returns>
        public virtual Encoding GetEncoding()
        {
            Encoding encoding = Encoding.UTF8;

            try
            {
                encoding = Encoding.GetEncoding(_characterSet.ToLower());
            }
            catch
            {
            }

            return encoding;
        }

        public virtual void Dispose()
        {
            if (_responseStream != null)
            {
                _responseStream.Dispose();
            }
        }
    }
}
