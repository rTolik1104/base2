using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using DirRX.MailAdapterSolution.ApprovalAssignment;

namespace DirRX.MailAdapterSolution
{
  partial class ApprovalAssignmentServerHandlers
  {

    public override void BeforeComplete(Sungero.Workflow.Server.BeforeCompleteEventArgs e)
    {
      base.BeforeComplete(e);
      if (_obj.CompletedBy.IsSystem.HasValue && _obj.CompletedBy.IsSystem.Value)
       e.Result = MailAdapterSolution.ActionItemExecutionAssignments.Resources.JobExecutedByMail;
    }
  }

}