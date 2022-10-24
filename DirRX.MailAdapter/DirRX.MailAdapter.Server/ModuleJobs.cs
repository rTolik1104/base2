using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace DirRX.MailAdapter.Server
{
  public class ModuleJobs
  {

    /// <summary>
    /// Отправка уведомления о заданиях.
    /// </summary>
    public virtual void SendMailNotification()
    {
      Functions.Module.SendMailNotification();
    }

  }
}