using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using DirRX.MailAdapterSolution.Employee;

namespace DirRX.MailAdapterSolution
{
  partial class EmployeeClientHandlers
  {

    public override void Showing(Sungero.Presentation.FormShowingEventArgs e)
    {
      base.Showing(e);
      DirRX.MailAdapterSolution.Functions.Employee.SetPropertiesDependsNeedNotifyNewAssignments(_obj);
      DirRX.MailAdapterSolution.Functions.Employee.SetPropertiesDependsIsExecuteThroughMail(_obj);
    }
  }

}