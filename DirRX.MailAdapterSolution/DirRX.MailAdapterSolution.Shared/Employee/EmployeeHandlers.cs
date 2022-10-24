using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using DirRX.MailAdapterSolution.Employee;

namespace DirRX.MailAdapterSolution
{
  partial class EmployeeSharedHandlers
  {

    public virtual void IsExecuteThroughMailChanged(Sungero.Domain.Shared.BooleanPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
        
      if (e.NewValue.HasValue && !e.NewValue.Value)
        _obj.MailAdapterHTMLMailBodyDirRX = false;
      
      DirRX.MailAdapterSolution.Functions.Employee.SetPropertiesDependsIsExecuteThroughMail(_obj);
    }

    public override void NeedNotifyNewAssignmentsChanged(Sungero.Domain.Shared.BooleanPropertyChangedEventArgs e)
    {
      base.NeedNotifyNewAssignmentsChanged(e);
      
      if (e.NewValue == e.OldValue)
        return;
        
      if (e.NewValue.HasValue && !e.NewValue.Value)
      {
        _obj.IsExecuteThroughMail = false;
        _obj.MailAdapterHTMLMailBodyDirRX = false;
      }
      
      DirRX.MailAdapterSolution.Functions.Employee.SetPropertiesDependsNeedNotifyNewAssignments(_obj);
    }
  }
}