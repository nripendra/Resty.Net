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
using System.Text;
using System.Threading.Tasks;

namespace Resty.Net.Extensions
{
    //Credits:
    //http://jittuu.com/2012/3/26/TPL-with-Extension-for-Tranditional-Net-Async-Programming/
    //http://jittuu.com/2012/3/28/Implementing-Non-Blocking-IO-StreamReader-ReadToEndAsync-method/
    public static class StreamTaskParallelism
    {
        public static Task<int> CopyToAsync(this Stream source, Stream destination, byte[] buffer, int offset, int count, object state)
        {
            return Task.Factory.FromAsync<byte[], int, int, int>(source.BeginRead, source.EndRead, buffer, offset, count, state).ContinueWith<int>(task =>
            {
                if (task.IsFaulted)
                    return 0;
                else if (task.IsCanceled)
                    return 0;

                var read = task.Result;

                Task<int> remainingTask = null;
                if (read > 0)
                {
                    destination.Write(buffer, 0, read);
                    offset += read;
                    remainingTask = CopyToAsync(source, destination, buffer, offset, count - read, state);
                    return Task.Factory.ContinueWhenAll(new Task[] { remainingTask }, tasks => remainingTask.Result).Result;
                }
                else
                    return read;

            });
        }
    }
}
