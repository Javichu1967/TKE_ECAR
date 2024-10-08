using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using TK_ECAR.Models;

namespace TK_ECAR.Utils
{
    [HubName("ECAR_ProgressHub")]
    public class ECAR_ProgressHub : Hub
    {
        public string msg = string.Empty;
        public int count = 0;
        public ResumenImportacionModels incidencias = new ResumenImportacionModels();

        public void CallLongOperation()
        {
            //Declaración de los eventos que quiero en la vista.
            Clients.Caller.sendMessage(msg, count);
            Clients.Caller.sendMessageSubProcess(msg, count); 
            Clients.Caller.sendMessageFinished(incidencias);
        }
    }
}