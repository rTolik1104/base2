using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.RecordManagement.PreparingDraftResolutionAssignment;

namespace Sungero.RecordManagement
{
  partial class PreparingDraftResolutionAssignmentClientHandlers
  {

    public override void Refresh(Sungero.Presentation.FormRefreshEventArgs e)
    {
      _obj.State.Properties.Addressee.IsVisible = _obj.IsRework != true || Equals(_obj.Performer, _obj.Task.Author);
    }

    public override void Showing(Sungero.Presentation.FormShowingEventArgs e)
    {
      if (_obj.IsRework == true && !Equals(_obj.Performer, _obj.Task.Author))
        e.HideAction(_obj.Info.Actions.Forward);
      
      // Скрывать результат выполнения "Вернуть инициатору" для задач, стартованных на ранних версиях схемы или в рамках согласования по регламенту.
      var schemeSupportsRework = Functions.DocumentReviewTask.SchemeVersionSupportsRework(_obj.Task);
      var reviewStartedFromApproval = Functions.DocumentReviewTask.ReviewStartedFromApproval(_obj.Task);
      if (!schemeSupportsRework || reviewStartedFromApproval)
        e.HideAction(_obj.Info.Actions.ForRework);
    }

  }
}