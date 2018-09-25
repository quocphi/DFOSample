﻿namespace Dfo.Sample.Core.Web.Base
{
    /// <summary>
    /// Provide the common component across multi layers in application
    /// </summary>
    public class SessionContext:ISessionContext
    {
        public string TenantId { get; set; }
    }
}