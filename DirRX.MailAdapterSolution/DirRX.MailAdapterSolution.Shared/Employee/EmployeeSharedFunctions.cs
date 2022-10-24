using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using DirRX.MailAdapterSolution.Employee;

namespace DirRX.MailAdapterSolution.Shared
{
  partial class EmployeeFunctions
  {
    /// <summary>
    /// Актуализировать параметры свойств, зависящих от NeedNotifyNewAssignments.
    /// </summary>
    public virtual void SetPropertiesDependsNeedNotifyNewAssignments()
    {
      _obj.State.Properties.IsExecuteThroughMail.IsEnabled = _obj.NeedNotifyNewAssignments.HasValue ? _obj.NeedNotifyNewAssignments.Value : false;
    }
    
    /// <summary>
    /// Актуализировать параметры свойств, зависящих от IsExecuteThroughMail.
    /// </summary>
    public virtual void SetPropertiesDependsIsExecuteThroughMail()
    {
      _obj.State.Properties.MailAdapterHTMLMailBodyDirRX.IsEnabled = _obj.IsExecuteThroughMail.HasValue ? _obj.IsExecuteThroughMail.Value : false;
    }
  }
}