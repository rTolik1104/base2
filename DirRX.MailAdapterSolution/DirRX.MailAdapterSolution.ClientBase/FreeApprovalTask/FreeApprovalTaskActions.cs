using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using DirRX.MailAdapterSolution.FreeApprovalTask;

namespace DirRX.MailAdapterSolution.Client
{
  partial class FreeApprovalTaskActions
  {
    public virtual void SendMailDirRX(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      DirRX.MailAdapter.PublicFunctions.MailLogs.Remote.CreateMailLogByTask(_obj, true);
      e.AddInformation(MailAdapter.MailLogses.Resources.MailInQueue);
    }

    public virtual bool CanSendMailDirRX(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

    public virtual void ShowMailLogsDirRX(Sungero.Domain.Client.ExecuteActionArgs e)
    {
      var logs = DirRX.MailAdapter.PublicFunctions.MailLogs.Remote.GetMailLogs(_obj);
      logs.Show();
    }

    public virtual bool CanShowMailLogsDirRX(Sungero.Domain.Client.CanExecuteActionArgs e)
    {
      return true;
    }

  }

}