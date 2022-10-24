using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Domain.Initialization;
using Sungero.Docflow;

namespace DirRX.MailAdapter.Server
{
  public partial class ModuleInitializer
  {

    public override void Initializing(Sungero.Domain.ModuleInitializingEventArgs e)
    {
      // Выдача прав всем пользователям.
      var allUsers = Sungero.CoreEntities.Roles.AllUsers;
      if (allUsers != null)
      {
        // Выдача прав на справочники.
        Logger.Debug("Init: Grant rights on databooks to all users.");
        MailAdapter.Functions.MailLogs.GrantRightsOnMailLogs(allUsers);
      }
      
      CreateAssignmentsTables();
    }

    /// <summary>
    /// Генерация таблиц для хранения очереди заданий на выполнение.
    /// </summary>
    public void CreateAssignmentsTables()
    {
      InitializationLogger.Debug("Init: Create assignments queue tables.");
      Sungero.Docflow.PublicFunctions.Module.ExecuteSQLCommandFormat(Queries.Module.CreateTableAssignmentsQueue,
                                                                     new[] { Constants.Module.AssignmentsQueue.AssignmentsQueueTableName });
      Sungero.Docflow.PublicFunctions.Module.ExecuteSQLCommandFormat(Queries.Module.CreateTableAssignmentsAttachments,
                                                                     new[] { Constants.Module.AssignmentsQueue.AssignmentAttachmentsTableName });
    }
  }
}
