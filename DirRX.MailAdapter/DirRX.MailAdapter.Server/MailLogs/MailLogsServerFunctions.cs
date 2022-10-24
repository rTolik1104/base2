using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using DirRX.MailAdapter.MailLogs;

namespace DirRX.MailAdapter.Server
{
  partial class MailLogsFunctions
  {
    
    /// <summary>
    /// Выдать права всем пользователям на справочник MailLogs.
    /// </summary>
    /// <param name="allUsers">Группа "Все пользователи".</param>
    [Remote]
    public static void GrantRightsOnMailLogs(IRole allUsers)
    {
      Logger.Debug("Init: Grant rights on Maillogs to all users.");
      MailLogses.AccessRights.Grant(allUsers, DefaultAccessRightsTypes.FullAccess);
      MailLogses.AccessRights.Save();
    }
    
    /// <summary>
    /// Возвращает журнал почтовой рассылки по конкретному заданию.
    /// </summary>
    /// <param name="task">Искомое задание.</param>
    /// <returns>Журнал рассылки.</returns>
    [Remote(IsPure = true), Public]
    public static IQueryable<DirRX.MailAdapter.IMailLogs> GetMailLogs(Sungero.Workflow.ITask task)
    {
      return DirRX.MailAdapter.MailLogses.GetAll().Where(l => l.Assignment.Task == task);
    }

    /// <summary>
    /// Создать новую запись в MailLogs.
    /// </summary>
    /// <param name="assignment">Задание по потокрому создается письмо.</param>
    /// <param name="isManual">Проверка, ручной запуск или нет.</param>
    /// <returns>Возвращается ссылка на созданную запись лога.</returns>
    [Remote, Public]
    public static DirRX.MailAdapter.IMailLogs CreateMailLog(Sungero.Workflow.IAssignmentBase assignment, bool isManual)
    {
        var logitem = DirRX.MailAdapter.MailLogses.Create();
        // Создать понятное наименование лога. Задается в шаблоне.
        logitem.Name = MailAdapter.Resources.LogNameTplFormat(assignment.Performer, assignment).ToString();
        // Обрезать имя, если длина превышает 500 симоволов (ограничено параметрами свойства).
        logitem.Name = logitem.Name.Length > 500 ? logitem.Name.Substring(0, 499) : logitem.Name;
        logitem.Assignment = assignment;
        logitem.Performer = assignment.Performer;
        logitem.ReqNotifDate = Calendar.Now;
        // Зафиксировать факт способа рассылки. Вручную из задачи, либо автоматически фоновым процессом.
        logitem.SendType = isManual ? MailAdapter.MailLogs.SendType.Manual : MailAdapter.MailLogs.SendType.Automatic;
        logitem.SendState = MailAdapter.MailLogs.SendState.InProcess;
        logitem.Save();
        return logitem;
    }
    
    /// <summary>
    /// Создать записи в MailLogs для всех активных заданий по задаче.
    /// </summary>
    /// <param name="task">Задача, по которой ищутся задания в работе и по ним создаются письма.</param>
    /// <param name="isManual">Проверка, ручной запуск или нет.</param>
    [Remote, Public]
    public static void CreateMailLogByTask(Sungero.Workflow.ITask task, bool isManual)
    {
      // Получить все активные задания, запущенные в рамках задачи task.
      var assignmentlist = Sungero.Workflow.Assignments.GetAll(a => (a.Task == task) && (a.Status == Sungero.Workflow.Assignment.Status.InProcess));
      // Выполнить рассылку по списку заданий assignmentlist
      foreach (var assignment in assignmentlist)
      {
        CreateMailLog(assignment, true);
      }
      return;
    }
    
  }
}