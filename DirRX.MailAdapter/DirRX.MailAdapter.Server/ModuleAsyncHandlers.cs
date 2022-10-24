using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace DirRX.MailAdapter.Server
{
  public class ModuleAsyncHandlers
  {

    public virtual void ProcessAssignment(DirRX.MailAdapter.Server.AsyncHandlerInvokeArgs.ProcessAssignmentInvokeArgs args)
    {
      Logger.DebugFormat("Processing assignment queue Id = {0}", args.ID);
      var item = AssignmentQueues.GetAll(a => a.Id == args.ID).FirstOrDefault();
      
      if (item == null)
        return;
      
      if (item.Assignment != null && item.Assignment.Status == Sungero.Workflow.AssignmentBase.Status.InProcess)
      {
        if (!Locks.GetLockInfo(item.Assignment).IsLockedByOther)
        {
          // Создать замещение.
          var substitution = Functions.Module.CreateSubstitution(item.Assignment.Performer, Users.Current);
          Functions.Module.ApproveAttachedDocuments(item);
          
          // Вложить файлы из письма для простой задачи.
          if (Sungero.Workflow.SimpleAssignments.Is(item.Assignment))
            Functions.Module.CreateAttachments(item);
          
          // Выполнить задание.
          Functions.Module.CompleteAssigment(item);
          Logger.DebugFormat(MailAdapter.Resources.IncomingMailProcessedSuccessTemplate, item.MailInfo, item.Assignment.Id);
          
          Functions.Module.RemoveAssignmentFromQueue(item);
          
          // Удалить замещение.
          Functions.Module.DeleteSubstitution(substitution);
        }
        else
        {
          args.Retry = true;
          Logger.DebugFormat("Assignment Id queue = {0} is locked. Need to retry", args.ID);
        }
      }
      else
      {
        Functions.Module.RemoveAssignmentFromQueue(item);
        Logger.DebugFormat("Remove assignment queue Id = {0} from queue", args.ID);
      }
    }

  }
}