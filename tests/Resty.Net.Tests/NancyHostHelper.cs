using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Nancy.Hosting.Self;

namespace Resty.Net.Tests
{
    public class NancyHostHelper
    {
        protected Uri _uri = new Uri("http://localhost:50001");
        private NancyHost _Nancy;
        private static int port = 50001;

        public Uri Start()
        {
            bool nancyStarted = false;
            // Need to retry in order to ensure that we properly startup after any failures
            for (var i = 0; i < 3; i++)
            {
                _Nancy = new NancyHost(_uri);

                try
                {
                    _Nancy.Start();
                    nancyStarted = true;
                    break;
                }
                catch (HttpListenerException)
                {
                    UriBuilder ub = new UriBuilder(_uri);
                    ub.Port = ++port;
                    _uri = ub.Uri;
                }
                catch
                {
                    try
                    {
                        _Nancy.Stop();
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            if (!nancyStarted)
            {
                //Don't allow to run the tests if Nancy not started.
                throw new Exception();
            }

            return _uri;
        }

        public void Stop()
        {
            try
            {
                _Nancy.Stop();
                _Nancy = null;
            }
            catch { }
        }
    }
}
