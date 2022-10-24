using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using DirRX.MailAdapterSolution.Employee;

namespace DirRX.MailAdapterSolution
{
  partial class EmployeeServerHandlers
  {

    public override void Created(Sungero.Domain.CreatedEventArgs e)
    {
      base.Created(e);
      _obj.IsExecuteThroughMail = false;
      _obj.MailAdapterHTMLMailBodyDirRX = false;
    }
  }

}