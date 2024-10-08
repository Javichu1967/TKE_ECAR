using log4net;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TK_ECAR.Monitorizacion
{

   
    [PSerializable]
    public sealed class LogExceptionAttribute : OnExceptionAspect
    {
        //
        public override void OnException(MethodExecutionArgs args)
        {
            
            ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            logger.Error(args.Exception); 
            args.FlowBehavior = FlowBehavior.Continue;
        }
    }
}
