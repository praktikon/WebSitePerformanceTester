using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;

namespace WebSitePerformanceTester.Infrastructure
{
    public class HubActivator : IHubActivator
    {
        private readonly IKernel container;

        public HubActivator(IKernel container)
        {
            this.container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            return (IHub)container.Get(descriptor.HubType);
        }
    }
}