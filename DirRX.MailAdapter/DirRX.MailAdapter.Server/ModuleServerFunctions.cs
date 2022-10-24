using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Sungero.Core;
using Sungero.CoreEntities;
using Sungero.Workflow;
using Sungero.Docflow;
using Sungero.Company;
using Nustache.Core;
// CORE: Sungero.Domain.LinqExpressions: Запрещено использование класса.
using Sungero.Domain;
// CORE: CommonLibrary: Запрещено использование класса.
// CORE: Sungero.Domain.LinqExpressions: Запрещено использование класса.
using Sungero.Domain.LinqExpressions;
using CommonLibrary;

namespace DirRX.MailAdapter.Server
{
  public class ModuleFunctions
  {
    
    #region Прикладные методы
    
    /// <summary>
    /// Проверить тип задания.
    /// </summary>
    /// <param name="typeName">Наименование типа задания.</param>
    /// <returns>Возвращает true, если задание: простое, свободное согласование, согласование по регламенту, на исполнение поручения, на отправку документа контрагенту, на ознакомление.
    /// Иначе - false. </returns>
    public virtual bool CheckAssignmentType(string typeName)
    {
      return (typeName == Constants.Module.SimpleAssignmentName) ||
        (typeName == Constants.Module.FreeApprovalAssignmentName) ||
        (typeName == Constants.Module.ApprovalAssignmentName) ||
        (typeName == Constants.Module.ActionItemExecutionAssignmentName) ||
        (typeName == Constants.Module.ActionItemSupervisorAssignmentName) ||
        (typeName == Constants.Module.ApprovalSendingAssignmentName) ||
        (typeName == Constants.Module.AcquaintanceAssignmentName) ||
        (typeName == Constants.Module.ApprovalSimpleAssignmentName) ||
        (typeName == Constants.Module.ApprovalCheckingAssignmentName);
    }
    
    /// <summary>
    /// Получить правило выполнения задания.
    /// </summary>
    /// <param name="assignment">Задание.</param>
    /// <returns>Правило выполнения задания.</returns>
    public virtual Structures.Module.MailProcessAssignmentType GetMailProcessAssignmentType(IAssignmentBase assignment)
    {
      var typeName = assignment.Info.Name;
      // Простое задание.
      if (typeName == Constants.Module.SimpleAssignmentName)
      {
        var resultItems = new List<Structures.Module.AllowResultData>() {
          Structures.Module.AllowResultData.Create("Complete", Resources.ReplyInstructionTextSimpleAssignmentComplete, false, string.Empty) };
        AddCustomActionResults(Constants.Module.SimpleAssignmentName, resultItems);
        return Structures.Module.MailProcessAssignmentType.Create(Constants.Module.SimpleAssignmentName, Resources.IncomingInstructionTextSimpleAssignment, resultItems);
      }
      
      // Cвободное согласование.
      if (typeName == Constants.Module.FreeApprovalAssignmentName)
      {
        var resultItems = new List<Structures.Module.AllowResultData>() {
          Structures.Module.AllowResultData.Create(Sungero.Docflow.FreeApprovalAssignment.Result.Approved.Value,
                                                   Resources.ReplyInstructionTextApprovalAssignmentApproved, false, string.Empty),
          Structures.Module.AllowResultData.Create(Sungero.Docflow.FreeApprovalAssignment.Result.ForRework.Value,
                                                   Resources.ReplyInstructionTextApprovalAssignmentForRework, false, string.Empty) };
        AddCustomActionResults(Constants.Module.FreeApprovalAssignmentName, resultItems);
        return Structures.Module.MailProcessAssignmentType.Create(Constants.Module.FreeApprovalAssignmentName, Resources.IncomingInstructionTextApprovalAssignment, resultItems);
      }
      // Согласование по регламенту.
      if (typeName == Constants.Module.ApprovalAssignmentName)
      {
        var resultItems = new List<Structures.Module.AllowResultData>() {
          Structures.Module.AllowResultData.Create(Sungero.Docflow.ApprovalAssignment.Result.Approved.Value,
                                                   Resources.ReplyInstructionTextApprovalAssignmentApproved, false, string.Empty),
          Structures.Module.AllowResultData.Create(Sungero.Docflow.ApprovalAssignment.Result.ForRevision.Value,
                                                   Resources.ReplyInstructionTextApprovalAssignmentForRework, false, string.Empty) };
        AddCustomActionResults(Constants.Module.ApprovalAssignmentName, resultItems);
        return Structures.Module.MailProcessAssignmentType.Create(Constants.Module.ApprovalAssignmentName, Resources.IncomingInstructionTextApprovalAssignment, resultItems);
      }
      // Простое задание на согласование по регламенту.
      if (typeName == Constants.Module.ApprovalSimpleAssignmentName)
      {
        var resultItems = new List<Structures.Module.AllowResultData>() {
          Structures.Module.AllowResultData.Create(Sungero.Docflow.ApprovalSimpleAssignment.Result.Complete.Value,
                                                   Resources.ReplyInstructionTextSimpleAssignmentComplete, false, string.Empty) };
        AddCustomActionResults(Constants.Module.ApprovalSimpleAssignmentName, resultItems);
        return Structures.Module.MailProcessAssignmentType.Create(Constants.Module.ApprovalSimpleAssignmentName, Resources.IncomingInstructionTextApprovalAssignment, resultItems);
      }
      // Простое задание на согласование по регламенту с доработкой.
      if (typeName == Constants.Module.ApprovalCheckingAssignmentName)
      {
        var resultItems = new List<Structures.Module.AllowResultData>() {
          Structures.Module.AllowResultData.Create(Sungero.Docflow.ApprovalCheckingAssignment.Result.Accept.Value,
                                                   Resources.ReplyInstructionTextApprovalAssignmentApproved, false, string.Empty),
          Structures.Module.AllowResultData.Create(Sungero.Docflow.ApprovalCheckingAssignment.Result.ForRework.Value,
                                                   Resources.ReplyInstructionTextApprovalAssignmentForRework, false, string.Empty) };
        AddCustomActionResults(Constants.Module.ApprovalCheckingAssignmentName, resultItems);
        return Structures.Module.MailProcessAssignmentType.Create(Constants.Module.ApprovalCheckingAssignmentName, Resources.IncomingInstructionTextActionItemSupervisorAssignment, resultItems);
      }
      // Задание на исполнения поручения.
      if (typeName == Constants.Module.ActionItemExecutionAssignmentName)
      {
        var resultItems = new List<Structures.Module.AllowResultData>() {
          Structures.Module.AllowResultData.Create(Sungero.RecordManagement.ActionItemExecutionAssignment.Result.Done.Value,
                                                   Resources.ReplyInstructionTextActionItemExecutionAssignment, false, string.Empty) };
        AddCustomActionResults(Constants.Module.ActionItemExecutionAssignmentName, resultItems);
        return Structures.Module.MailProcessAssignmentType.Create(Constants.Module.ActionItemExecutionAssignmentName, Resources.IncomingInstructionTextActionItemExecutionAssignment, resultItems);
      }
      // Рассмотрение контролером.
      if (typeName == Constants.Module.ActionItemSupervisorAssignmentName)
      {
        var resultItems = new List<Structures.Module.AllowResultData>() {
          Structures.Module.AllowResultData.Create(Sungero.RecordManagement.ActionItemSupervisorAssignment.Result.Agree.Value,
                                                   Resources.ReplyInstructionTextActionItemSupervisorAssignmentAgree, false, string.Empty),
          Structures.Module.AllowResultData.Create(Sungero.RecordManagement.ActionItemSupervisorAssignment.Result.ForRework.Value,
                                                   Resources.ReplyInstructionTextActionItemSupervisorAssignmentForRework, false, string.Empty) };
        AddCustomActionResults(Constants.Module.ActionItemSupervisorAssignmentName, resultItems);
        return Structures.Module.MailProcessAssignmentType.Create(Constants.Module.ActionItemSupervisorAssignmentName, Resources.IncomingInstructionTextActionItemSupervisorAssignment, resultItems);
      }
      // Задание на отправку документа контрагенту.
      if (typeName == Constants.Module.ApprovalSendingAssignmentName)
      {
        var resultItems = new List<Structures.Module.AllowResultData>() {
          Structures.Module.AllowResultData.Create(Sungero.Docflow.ApprovalSendingAssignment.Result.Complete.Value,
                                                   Resources.ReplyInstructionTextApprovalSendingAssignment, false, string.Empty) };
        AddCustomActionResults(Constants.Module.ApprovalSendingAssignmentName, resultItems);
        return Structures.Module.MailProcessAssignmentType.Create(Constants.Module.ApprovalSendingAssignmentName, Resources.IncomingInstructionTextApprovalSendingAssignment, resultItems);
      }
      
      // Задание на ознакомление с документом.
      if (typeName == Constants.Module.AcquaintanceAssignmentName)
      {
        var resultItems = new List<Structures.Module.AllowResultData>() {
          Structures.Module.AllowResultData.Create(Sungero.RecordManagement.AcquaintanceAssignment.Result.Acquainted.Value,
                                                   Resources.ReplyInstructionTextAcquaintanceAssignment, false, string.Empty) };
        AddCustomActionResults(Constants.Module.AcquaintanceAssignmentName, resultItems);
        return Structures.Module.MailProcessAssignmentType.Create(Constants.Module.AcquaintanceAssignmentName, Resources.IncomingInstructionTextAcquaintanceAssignment, resultItems);
      }
      
      return null;
    }

    /// <summary>
    /// Получить группу "Администраторы".
    /// </summary>
    /// <returns>Группа "Администраторы".</returns>
    private static IRole GetAdministratorsGroup()
    {
      return Roles.GetAll(g => g.Sid == Sungero.Domain.Shared.SystemRoleSid.Administrators).SingleOrDefault();
      
    }
    
    /// <summary>
    /// Создать замещение.
    /// </summary>
    /// <param name="user">Пользователь.</param>
    /// <param name="substitute">Замещающий пользователь.</param>
    /// <returns>Замещение.</returns>
    [Remote]
    public static ISubstitution CreateSubstitution(IUser user, IUser substitute)
    {
      if (!Substitutions.GetAll().Any(x => user.Equals(x.User) && substitute.Equals(x.Substitute)))
      {
        var substitution = Substitutions.Create();
        substitution.User = user;
        substitution.Substitute = substitute;
        substitution.IsSystem = true;
        substitution.Comment = string.Empty;
        substitution.Save();
        return substitution;
      }
      return null;
    }
    
    /// <summary>
    /// Удалить замещение.
    /// </summary>
    /// <param name="substitution">Замещение.</param>
    [Remote]
    public void DeleteSubstitution(ISubstitution substitution)
    {
      // Удалить замещение.
      if (Substitutions.GetAll().Any(x => substitution.Equals(x)))
      {
        Substitutions.Delete(substitution);
      }
    }
    
    /// <summary>
    /// Создает простой документ.
    /// </summary>
    /// <returns>Простой документ.</returns>
    [Remote]
    public ISimpleDocument CreateSimpleDocument()
    {
      return SimpleDocuments.Create();
    }
    
    /// <summary>
    /// Закодировать строку в base64.
    /// </summary>
    /// <param name="data">Строка.</param>
    /// <returns>Закодированная строка.</returns>
    private static string StringToBase64(string data)
    {
      byte[] byt = System.Text.Encoding.UTF8.GetBytes(data);
      var strModified = Convert.ToBase64String(byt);
      return strModified;
    }
    
    /// <summary>
    /// Раскодировать строку из base64.
    /// </summary>
    /// <param name="data">Данные.</param>
    /// <returns>Исходная строка.</returns>
    private static string Base64ToString(string data)
    {
      byte[] b = Convert.FromBase64String(data);
      var strOriginal = System.Text.Encoding.UTF8.GetString(b);
      return strOriginal;
    }
    
    #endregion
    
    #region Задублировано из базового решения
    
    /// <summary>
    /// Попытаться отправить письмо по заданию.
    /// </summary>
    /// <param name="assignment">Задание.</param>
    /// <param name="subject">Тема.</param>
    /// <param name="isExpired">Признак того, что задание является просроченным.</param>
    /// <param name="addressee">Получатель письма.</param>
    /// <param name="copies">Получатели копий письма.</param>
    /// <returns>True, если ошибок при отправке не было, иначе - False.</returns>
    public DirRX.MailAdapter.Structures.Module.IMailSendResult TrySendMailByAssignmentBase(IAssignmentBase assignment, string subject, bool isExpired, IEmployee addressee, System.Collections.Generic.IEnumerable<IEmployee> copies)
    {
      var needNotify = (isExpired ? addressee.NeedNotifyExpiredAssignments == true : addressee.NeedNotifyNewAssignments == true) &&
        addressee.Status != Sungero.CoreEntities.DatabookEntry.Status.Closed;
      copies = copies.Where(c => !c.Equals(addressee));
      var to = needNotify && !string.IsNullOrEmpty(addressee.Email) ? addressee.Email : null;
      var cc = copies.Select(e => e.Email).Where(email => !string.IsNullOrEmpty(email)).ToList();
      
      
      if (string.IsNullOrEmpty(to) && !cc.Any())
        return DirRX.MailAdapter.Structures.Module.MailSendResult.Create(true, false);
      
      bool isSendMailSuccess = false;
      try
      {
        Logger.DebugFormat("Sending mail by assignment with Id = {0}. Is expired: {1}", assignment.Id, isExpired);
        
        this.InternalSendMailByAssignmentBase(assignment, subject, isExpired, to, cc);
        
        if (!string.IsNullOrEmpty(to))
          Logger.DebugFormat("Mail to performer with Id = {0} has been sent", addressee.Id);
        else if (needNotify)
          Logger.DebugFormat("Performer with Id = {0} has no email", addressee.Id);
        
        foreach (var employee in copies)
        {
          if (!string.IsNullOrEmpty(employee.Email))
            Logger.DebugFormat("Mail to substitute with Id = {0} has been sent", employee.Id);
          else
            Logger.DebugFormat("Substitute with Id = {0} has no email", employee.Id);
        }
        
        isSendMailSuccess = true;
      }
      catch (FormatException ex)
      {
        Logger.ErrorFormat("Performer with Id = {0} or his substitute has incorrect email", ex, addressee.Id);
      }
      catch (Exception ex)
      {
        Logger.ErrorFormat("Error while sending mail to performer with Id = {0} or his substitute", ex, addressee.Id);
      }
      return DirRX.MailAdapter.Structures.Module.MailSendResult.Create(isSendMailSuccess, isSendMailSuccess && (!string.IsNullOrEmpty(to) || cc.Any()));
    }
    
    /// <summary>
    /// Отправить письмо по заданию.
    /// </summary>
    /// <param name="assignment">Задание.</param>
    /// <param name="subject">Тема.</param>
    /// <param name="isExpired">Признак того, что задание является просроченным.</param>
    /// <param name="to">Получатель письма.</param>
    /// <param name="cc">Получатели копий письма.</param>
    public void InternalSendMailByAssignmentBase(IAssignmentBase assignment, string subject, bool isExpired, string to, System.Collections.Generic.IEnumerable<string> cc)
    {
      var message = Mail.CreateMailMessage();
      message.Body = this.GenerateBodyBase(assignment, isExpired, cc.Any());
      message.IsBodyHtml = true;
      message.Subject = subject.Replace('\r', ' ').Replace('\n', ' ');
      if (!string.IsNullOrEmpty(to))
        message.To.Add(to);
      foreach (var email in cc.Where(e => !string.IsNullOrEmpty(e)))
        message.CC.Add(email);
      if (assignment.Importance == Sungero.Workflow.AssignmentBase.Importance.High)
        message.Priority = Sungero.Core.MailPriority.High;
      else if (assignment.Importance == Sungero.Workflow.AssignmentBase.Importance.Low)
        message.Priority = Sungero.Core.MailPriority.Low;
      
      // Создать вложения для писем.
      this.AddMailAttachments(assignment, message);

      Mail.Send(message);
    }
    
    /// <summary>
    /// Сгенерировать тело письма.
    /// </summary>
    /// <param name="assignment">Задание.</param>
    /// <param name="isExpired">Признак просроченного задания.</param>
    /// <param name="hasSubstitutions">Признак просрочки.</param>
    /// <returns>Тело письма.</returns>
    public virtual string GenerateBodyBase(IAssignmentBase assignment, bool isExpired, bool hasSubstitutions)
    {
      if (!Nustache.Core.Helpers.Contains("process_text"))
        Nustache.Core.Helpers.Register("process_text", ProcessText);
      
      var model = this.GenerateBodyModelBase(assignment, isExpired, hasSubstitutions);

      return Nustache.Core.Render.StringToString(Sungero.Docflow.Resources.MailTemplate, model);
    }
    
    /// <summary>
    /// Сгенерировать модель письма.
    /// </summary>
    /// <param name="assignment">Задание.</param>
    /// <param name="isExpired">Признак просроченного задания.</param>
    /// <param name="hasSubstitutions">Признак просрочки.</param>
    /// <returns>Модель письма.</returns>
    public virtual System.Collections.Hashtable GenerateBodyModelBase(IAssignmentBase assignment, bool isExpired, bool hasSubstitutions)
    {
      var model = new System.Collections.Hashtable();
      model["Assignment"] = assignment;
      model["Attachments"] = assignment.AllAttachments.Where(ent => ent.AccessRights.CanRead(assignment.Performer)).ToList();
      model["HasSubstitutions"] = hasSubstitutions;
      model["IsExpired"] = isExpired;
      model["AdministratorEmail"] = AdministrationSettings.AdministratorEmail;
      if (isExpired)
        model["MailingName"] = Sungero.Docflow.Resources.ExpiredAssignmentsMailingName.ToString().Replace(" ", "%20");
      else
        model["MailingName"] = Sungero.Docflow.Resources.NewAssignmentsMailingName.ToString().Replace(" ", "%20");
      if (!string.Equals(assignment.Subject, assignment.MainTask.Subject))
        model["Subject"] = assignment.MainTask.Subject;
      if (!Equals(assignment.Author, assignment.Performer))
        model["Author"] = assignment.Author;
      return model;
    }
    #endregion
    
    #region Обработка DCTS
    
    /// <summary>
    /// Выполнить задание.
    /// </summary>
    /// <param name="item">Выполняемое задание.</param>
    [Remote]
    public void CompleteAssigment(IAssignmentQueue item)
    {
      // Выполнить задание.
      this.ExecuteAssigment(item);
    }
    
    /// <summary>
    /// Выполнить задание.
    /// </summary>
    /// <param name="item">Очередь выполнения заданий.</param>
    private void ExecuteAssigment(IAssignmentQueue item)
    {
      var assignment = item.Assignment;
      assignment.ActiveText = item.ActiveText;
      var customAction = GetCustomActionForSend().Where(x => x.EntityTypeName == assignment.Info.Name && x.ActionName == item.Result).FirstOrDefault();
      if (customAction != null)
      {
        Sungero.Docflow.PublicFunctions.Module.GetServerEntityFunctionResult(assignment, customAction.ServerFunctionName, null);
        return;
      }
      // TODO. Выполнение простого задания. Т.к. он платформенный, то выполняется таким образом.
      if (SimpleAssignments.Is(assignment) && item.Result == "Complete")
        assignment.Complete(null);
      else
      {
        if (this.CheckAssignmentType(assignment.Info.Name))
          assignment.Complete(new Enumeration(item.Result));
      }
    }
    
    /// <summary>
    /// Сформировать текст для выполнения задания из тела письма и данных об отправителе.
    /// </summary>
    /// <param name="activeText">Тело письма.</param>
    /// <param name="address">Адрес отправителя.</param>
    /// <returns>Отфильтрованный текст.</returns>
    [Remote]
    public string FilterActiveText(string activeText, string address)
    {
      var employee = Employees.GetAll().Where(e => e.Email == address).FirstOrDefault();
      var senderInfo = employee != null ? employee.Name : address;
      var index = activeText.IndexOf(Constants.Module.MailBodySeparator);
      // Если в ответном письме полльзователь не указзал никакой текст, то вернуть пустую строку.
      var emailText = index > -1 ? activeText.Substring(0, index) : activeText;
      emailText = emailText.Replace(Constants.Module.NewLineHtmlTag, string.Empty);
      var result = string.Format(MailAdapter.Resources.ActiveTextTemplateFormat, senderInfo, emailText);
      
      return result;
    }
    
    #endregion
    
    #region Отправка письма.
    
    /// <summary>
    /// Отправить уведомления о заданиях.
    /// </summary>
    [Remote, Public]
    public virtual void SendMailNotification()
    {
      bool? needUpdateLastNotificationDate = null;
      var previousRun = GetLastNotificationDate();
      var notificationDate = Calendar.Now;
      try
      {
        
        var newAssignmentsResult = this.TrySendNewAssignmentsMailing(previousRun, notificationDate);
        var expiredAssignmentsResult = this.TrySendExpiredAssignmentsMailing(previousRun, notificationDate);
        needUpdateLastNotificationDate = newAssignmentsResult | expiredAssignmentsResult | !(newAssignmentsResult == false | expiredAssignmentsResult == false);
      }
      finally
      {
        if (needUpdateLastNotificationDate == true)
          UpdateLastNotificationDate(notificationDate);
        else
          Logger.Debug("Last notification date hasn't been changed");
      }
    }
    
    /// <summary>
    /// Запустить рассылку по новым заданиям.
    /// </summary>
    /// <param name="previousRun">Дата прошлого запуска рассылки.</param>
    /// <param name="notificationDate">Дата текущей рассылки.</param>
    /// <returns>True, если хотя бы одно письмо было отправлено, иначе - false.</returns>
    [Public]
    public virtual bool? TrySendNewAssignmentsMailing(DateTime previousRun, DateTime notificationDate)
    {
      Logger.Debug("Checking new assignments for mailing");
      var hasErrors = false;
      // CORE: строковая константа.
      var newAssignments = AssignmentBases
        .GetAll(a => previousRun <= a.Created && a.Created < notificationDate && a.IsRead == false && a.Status != Sungero.Workflow.AssignmentBase.Status.Aborted)
        .Expand("Performer")
        .ToList();
      newAssignments.RemoveAll(a => newAssignments.Any(duplicate => duplicate.Task == a.Task && duplicate.Performer == a.Performer && Sungero.Workflow.Notices.Is(a)));
      // Добавить к списку newAssignments задания, помеченные на отправку вручную в MailLogs.
      foreach (var logitem in DirRX.MailAdapter.MailLogses.GetAll(l => previousRun <= l.ReqNotifDate && l.ReqNotifDate <= notificationDate &&
                                                                  l.SendState == DirRX.MailAdapter.MailLogs.SendState.InProcess && l.SendType == DirRX.MailAdapter.MailLogs.SendType.Manual &&
                                                                  l.Assignment.Status == Sungero.Workflow.AssignmentBase.Status.InProcess))
      {
        newAssignments.Add(logitem.Assignment);
      }
      
      var anyMailSended = false;
      foreach (var assignment in newAssignments)
      {
        var employee = DirRX.MailAdapterSolution.Employees.As(assignment.Performer);
        if (employee == null)
          continue;
        if (employee.NeedNotifyNewAssignments != true)
          continue;
        var endDate = assignment.Created.Value.AddDays(-1);
        var substitutions = Substitutions.GetAll(s => Equals(s.User, employee))
          .Where(s => s.IsSystem != true)
          .Where(s => s.StartDate == null || s.StartDate.Value <= assignment.Created)
          .Where(s => s.EndDate == null || s.EndDate.Value >= endDate);
        
        var performers = DirRX.MailAdapterSolution.Employees.GetAll(r => r.NeedNotifyNewAssignments == true)
          .Where(e => substitutions.Any(s => Equals(s.Substitute, e)))
          .Where(e => e.Status != Sungero.CoreEntities.DatabookEntry.Status.Closed)
          .ToList();
        performers.Add(employee);
        
        var subject = Resources.MailSubjectFormat(assignment.Subject);
        
        // Проверить, зафиксирована ли в списке MailLogs отправка для этого задания.
        var logitem = DirRX.MailAdapter.MailLogses.GetAll(l => previousRun <= l.ReqNotifDate && l.ReqNotifDate <= notificationDate && l.Assignment == assignment).Count() > 0 ?
          DirRX.MailAdapter.MailLogses.GetAll(l => previousRun <= l.ReqNotifDate && l.ReqNotifDate <= notificationDate && l.Assignment == assignment).FirstOrDefault() :
          MailAdapter.PublicFunctions.MailLogs.Remote.CreateMailLog(assignment, false);
        
        // Зафиксировать успешность рассылки.
        var mailSendResult = employee.IsExecuteThroughMail == true ? this.TrySendMailByAssignment(assignment, subject, false, performers) :
          this.TrySendMailByAssignmentBase(assignment, subject, false, employee, performers);
        
        if (!mailSendResult.IsSendMailSuccess)
        {
          hasErrors = true;
          logitem.SendState = MailAdapter.MailLogs.SendState.Error;
        }
        else
        {
          logitem.SendState = MailAdapter.MailLogs.SendState.Sent;
        }
        logitem.Save();
        if (mailSendResult.IsSendMailSuccess && mailSendResult.IsAnyMailSended)
          anyMailSended = true;
      }
      if (!newAssignments.Any())
        Logger.Debug("No new assignments for mailing");
      else if (!anyMailSended)
        Logger.Debug("No subscribers for new assignments mailing");
      // Отправлены не все письма и есть ошибки.
      if (!anyMailSended && !hasErrors)
        return null;
      
      // Нет ошибок при отправке или хотя бы 1 письмо в задании отправлено кому нибудь из адресатов.
      return anyMailSended || !hasErrors;
    }
    
    /// <summary>
    /// Запустить рассылку по просроченным заданиям.
    /// </summary>
    /// <param name="previousRun">Дата прошлого запуска рассылки.</param>
    /// <param name="notificationDate">Дата текущей рассылки.</param>
    /// <returns>True, если хотя бы одно письмо было отправлено, иначе - false.</returns>
    [Public]
    public virtual bool? TrySendExpiredAssignmentsMailing(DateTime previousRun, DateTime notificationDate)
    {
      Logger.Debug("Checking expired assignments for mailing");
      // Наличие ошибок при отправке.
      var hasErrors = false;
      // CORE: строковая константа.
      var expiredAssignments = AssignmentBases
        .GetAll(a => a.Status == Sungero.Workflow.AssignmentBase.Status.InProcess  &&
                (a.Deadline.HasValue && a.Deadline.Value.HasTime() &&
                 previousRun <= a.Deadline && a.Deadline < notificationDate ||
                 a.Deadline.HasValue && !a.Deadline.Value.HasTime() &&
                 previousRun <= a.Deadline.Value.AddDays(1) && a.Deadline.Value.AddDays(1) < notificationDate))
        .Expand("Performer")
        .ToList();
      
      // Добавить к списку newAssignments задания, помеченные на отправку вручную в MailLogs.
      foreach (var logitem in DirRX.MailAdapter.MailLogses.GetAll(l => previousRun <= l.ReqNotifDate && l.ReqNotifDate < notificationDate &&
                                                                  l.SendState != DirRX.MailAdapter.MailLogs.SendState.Sent && l.SendType == DirRX.MailAdapter.MailLogs.SendType.Manual &&
                                                                  l.Assignment.Status != Sungero.Workflow.AssignmentBase.Status.InProcess && (l.Assignment.Deadline.HasValue && l.Assignment.Deadline.Value.HasTime() &&
                                                                                                                                              l.Assignment.Deadline < notificationDate || l.Assignment.Deadline.HasValue && !l.Assignment.Deadline.Value.HasTime() &&
                                                                                                                                              l.Assignment.Deadline.Value.AddDays(1) < notificationDate)))
      {
        expiredAssignments.Add(logitem.Assignment);
      }
      
      var anyMailSended = false;
      
      foreach (var assignment in expiredAssignments)
      {
        var employee = DirRX.MailAdapterSolution.Employees.As(assignment.Performer);
        if (employee == null)
          continue;
        if (employee.NeedNotifyExpiredAssignments != true)
          continue;

        var endDate = notificationDate.AddDays(-1);
        var substitutions = Substitutions.GetAll(s => Equals(s.User, employee))
          .Where(s => s.IsSystem != true)
          .Where(s => s.StartDate == null || s.StartDate.Value <= notificationDate)
          .Where(s => s.EndDate == null || s.EndDate.Value >= endDate);
        
        var performers = DirRX.MailAdapterSolution.Employees.GetAll(r => r.NeedNotifyNewAssignments == true)
          .Where(e => substitutions.Any(s => Equals(s.Substitute, e)))
          .Where(e => e.Status != Sungero.CoreEntities.DatabookEntry.Status.Closed)
          .ToList();
        performers.Add(employee);
        
        var subject = Sungero.Docflow.Resources.ExpiredAssignmentMailSubjectFormat(this.GetAssignmentTypeName(assignment).ToLower(), this.GetAuthorSubjectPart(assignment), assignment.Subject);
        
        // Проверить, зафиксирована ли в списке MailLogs отправка для этого задания.
        var logitem = DirRX.MailAdapter.MailLogses.GetAll(l => previousRun <= l.ReqNotifDate && l.ReqNotifDate <= notificationDate &&
                                                          l.Assignment == assignment).FirstOrDefault();
        // Если запись в логе не зафиксирована - зафиксировать.
        if (logitem == null)
        {
          logitem = MailAdapter.PublicFunctions.MailLogs.Remote.CreateMailLog(assignment, false);
        }
        
        // Зафиксировать успешность рассылки.
        var mailSendResult = employee.IsExecuteThroughMail == true ? this.TrySendMailByAssignment(assignment, subject, true, performers) :
          this.TrySendMailByAssignmentBase(assignment, subject, true, employee,  performers);
        
        if (!mailSendResult.IsSendMailSuccess)
        {
          hasErrors = true;
          logitem.SendState = MailAdapter.MailLogs.SendState.Error;
        }
        else
        {
          logitem.SendState = MailAdapter.MailLogs.SendState.Sent;
        }
        logitem.Save();
        
        if (mailSendResult.IsSendMailSuccess && mailSendResult.IsAnyMailSended)
          anyMailSended = true;
      }
      if (!expiredAssignments.Any())
        Logger.Debug("No expired assignments for mailing");
      else if (!anyMailSended)
        Logger.Debug("No subscribers for expired assignments mailing");
      if (!anyMailSended && !hasErrors)
        return null;
      return anyMailSended || !hasErrors;
    }
    
    /// <summary>
    /// Получить локализованное имя типа задания.
    /// </summary>
    /// <param name="assignment">Базовое задание.</param>
    /// <returns>Имя типа задания.</returns>
    private string GetAssignmentTypeName(IAssignmentBase assignment)
    {
      if (Notices.Is(assignment))
        return Notices.Info.LocalizedName;
      else if (ReviewAssignments.Is(assignment))
        return ReviewAssignments.Info.LocalizedName;
      else
        return Assignments.Info.LocalizedName;
    }
    
    /// <summary>
    /// Получить часть темы письма, которая содержит автора задания.
    /// </summary>
    /// <param name="assignment">Задание.</param>
    /// <returns>Часть темы письма с автором задания.</returns>
    private string GetAuthorSubjectPart(IAssignmentBase assignment)
    {
      if (Equals(assignment.Author, assignment.Performer))
        return string.Empty;

      // CORE: строковая константа.
      return string.Format(" {0} {1}", Sungero.Docflow.Resources.From, this.GetFormattedUserNameInGenitive(assignment.Author.DisplayValue));
    }
    
    /// <summary>
    /// Получить форматированное имя пользователя в винительном падеже.
    /// </summary>
    /// <param name="userName">Имя пользователя.</param>
    /// <returns>Форматированное имя пользователя.</returns>
    private string GetFormattedUserNameInGenitive(string userName)
    {
      PersonFullName personalData;
      var result = userName;
      if (PersonFullName.TryParse(result, out personalData) && !string.IsNullOrEmpty(personalData.MiddleName))
      {
        personalData.DisplayFormat = PersonFullNameDisplayFormat.LastNameAndInitials;
        result = CaseConverter.ConvertPersonFullNameToTargetDeclension(personalData, Sungero.Core.DeclensionCase.Genitive);
      }
      return result;
    }
    
    /// <summary>
    /// Попытаться отправить письмо по заданию.
    /// </summary>
    /// <param name="assignment">Задание.</param>
    /// <param name="subject">Тема.</param>
    /// <param name="isExpired">Признак того, что задание является просроченным.</param>
    /// <param name="addressees">Получатели письма.</param>
    /// <returns> Структура c двумя переменными bool: 1. True - если было отправлено хотя бы одно письмо. 2. True -  если при отправке не было ошибок.</returns>
    [Public]
    public virtual Structures.Module.IMailSendResult TrySendMailByAssignment(IAssignmentBase assignment, string subject, bool isExpired, System.Collections.Generic.List<DirRX.MailAdapterSolution.IEmployee> addressees)
    {
      var result = Structures.Module.MailSendResult.Create();
      // Получить список исполнителей, которым необходимо отправлять письма.
      var mailingList = addressees.Where(e => !string.IsNullOrEmpty(e.Email))
        .Where(r => r.Status != Sungero.CoreEntities.DatabookEntry.Status.Closed && r.IsExecuteThroughMail == true)
        .Where(t => isExpired ? t.NeedNotifyExpiredAssignments == true : t.NeedNotifyNewAssignments == true);

      // Если нет подходящих исполнителей.
      if (!mailingList.Any())
      {
        Logger.DebugFormat("Sending mail assignment Id = {0} is cancelled: suitable performers or substitutes are not found ", assignment.Id);
        result.IsSendMailSuccess = true;
        result.IsAnyMailSended = false;
        return result;
      }
      
      result.IsSendMailSuccess = false;
      try
      {
        Logger.DebugFormat("Sending mail by assignment with Id = {0}. Is expired: {1}", assignment.Id, isExpired);
        // Отправить письма получателям.
        foreach (var addresse in mailingList)
        {
          this.InternalSendMailByAssignment(assignment, subject, isExpired, addresse.Email, addresse.MailAdapterHTMLMailBodyDirRX);
          Logger.DebugFormat("Mail to performer or substitute with Id = {0} has been sent", addresse.Id);
          result.IsSendMailSuccess = true;
        }
      }
      catch (FormatException ex)
      {
        Logger.ErrorFormat("Performer with Id = {0} or substitute has incorrect email", ex, assignment.Performer.Id);
      }
      catch (Exception ex)
      {
        Logger.ErrorFormat("Error while sending mail to performer with Id = {0} or his substitute", ex, assignment.Performer.Id);
      }
      // Отправлено письмо хотя бы одному адресату или получателю копии письма.
      result.IsAnyMailSended = result.IsSendMailSuccess && mailingList.Any();
      return result;
    }
    
    /// <summary>
    /// Отправить письмо по заданию.
    /// </summary>
    /// <param name="assignment">Задание.</param>
    /// <param name="subject">Тема.</param>
    /// <param name="isExpired">Признак того, что задание является просроченным.</param>
    /// <param name="to">Получатель письма.</param>
    /// <param name="htmlMailBody">Признак того, что почтовый клиент требует html разметку тела ответного письма.</param>
    private void InternalSendMailByAssignment(IAssignmentBase assignment, string subject, bool isExpired, string to, bool? htmlMailBody)
    {
      var message = Mail.CreateMailMessage();
      message.Body = this.GenerateBody(assignment, isExpired, htmlMailBody);
      message.IsBodyHtml = true;
      message.Subject = subject.Replace('\r', ' ').Replace('\n', ' ');
      if (!string.IsNullOrEmpty(to))
        message.To.Add(to);
      if (assignment.Importance == Sungero.Workflow.AssignmentBase.Importance.High)
        message.Priority = Sungero.Core.MailPriority.High;
      else if (assignment.Importance == Sungero.Workflow.AssignmentBase.Importance.Low)
        message.Priority = Sungero.Core.MailPriority.Low;
      
      // Создать вложения для писем.
      this.AddMailAttachments(assignment, message);

      Mail.Send(message);
    }
    
    /// <summary>
    /// Добавить вложения в исходящее письмо.
    /// </summary>
    /// <param name="assignment">Задание.</param>
    /// <param name="message">Сообщение.</param>
    private void AddMailAttachments(IAssignmentBase assignment, Sungero.Core.IEmailMessage message)
    {
      var logoStream = new MemoryStream(Sungero.Core.SystemInfo.GetLogo());
      var attachment = message.AddAttachment(logoStream, "logo.png@01D004A6.A303C580");
      attachment.IsInline = true;
      attachment.MediaType = "image/png";
      // Список всех заданий, по которым необходимо вкладывать тело документа в письмо.
      if (this.CheckAssignmentType(assignment.Info.Name))
      {
        foreach (var item in assignment.AllAttachments.Where(x => Sungero.Content.ElectronicDocuments.Is(x)))
        {
          var document = Sungero.Content.ElectronicDocuments.As(item);
          if (document.HasVersions)
            message.AddAttachment(document.LastVersion);
        }
      }
    }
    
    /// <summary>
    /// Сгенерировать тело письма.
    /// </summary>
    /// <param name="assignment">Задание.</param>
    /// <param name="isExpired">Признак просроченного задания.</param>
    /// <param name="htmlMailBody">Признак того, что почтовый клиент требует html разметку тела ответного письма.</param>
    /// <returns>Тело письма.</returns>
    [Public]
    public virtual string GenerateBody(IAssignmentBase assignment, bool isExpired, bool? htmlMailBody)
    {
      // CORE: строковая константа.
      if (!Helpers.Contains("process_text"))
        Helpers.Register("process_text", ProcessText);
      
      var model = new System.Collections.Hashtable();
      // CORE: строковая константа.
      model["Assignment"] = assignment;
      // CORE: строковая константа.
      var attachments = assignment.AllAttachments.Where(ent => ent.AccessRights.CanRead(assignment.Performer)).ToList();
      model["Attachments"] = attachments;
      model["HasAttachments"] = attachments.Any();
      // CORE: строковая константа.
      model["IsExpired"] = isExpired;
      // CORE: строковая константа.
      model["AdministratorEmail"] = Sungero.Domain.AdministrationSettings.AdministratorEmail;
      
      if (isExpired)
        // CORE: строковая константа.
        model["MailingName"] = Sungero.Docflow.Resources.ExpiredAssignmentsMailingName.ToString().Replace(" ", "%20");
      else
        // CORE: строковая константа.
        model["MailingName"] = Sungero.Docflow.Resources.NewAssignmentsMailingName.ToString().Replace(" ", "%20");
      
      if (!string.Equals(assignment.Subject, assignment.MainTask.Subject))
        // CORE: строковая константа.
        model["Subject"] = assignment.MainTask.Subject;
      if (!Equals(assignment.Author, assignment.Performer))
        // CORE: строковая константа.
        model["Author"] = assignment.Author;
      // Гиперссылки на выполнения задания.
      var hyperlinkResults = this.GetHyperlinkResults(assignment, htmlMailBody);
      model["HyperlinkResults"] = hyperlinkResults;
      model["HasHyperlinkResults"] = hyperlinkResults.Any();
      
      var mailProcessAssignmentType = GetMailProcessAssignmentType(assignment);
      model["HeaderBodyMail"] = mailProcessAssignmentType != null ? mailProcessAssignmentType.IncomingInstructionText.Replace(Environment.NewLine, "<br>") : string.Empty;
      
      return Nustache.Core.Render.StringToString(DirRX.MailAdapter.Resources.MailTemplate, model);
    }
    
    /// <summary>
    /// Получить значение адреса эл.почты для ответного письма.
    /// </summary>
    /// <returns>Электронный адрес.</returns>
    public virtual string ReplyEmailAdress()
    {
      return GetServerConfigValue("ADMINISTRATOR_EMAIL_FOR_DCTS");
    }
    
    /// <summary>
    /// Получить значение из серверного конфигурационного файла по имени элемента.
    /// </summary>
    /// <param name="elementName">Имя элемента.</param>
    /// <returns>Значение элемента.</returns>
    private string GetServerConfigValue(string elementName)
    {
      var customConfigSettingsName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_CustomConfigSettings.xml");
      Logger.DebugFormat("Путь до _CustomConfigSettings:{0}", customConfigSettingsName);
      if (!File.Exists(customConfigSettingsName))
        throw new FileNotFoundException(MailAdapter.Resources.CustomConfigSettingNotFoundTemplateFormat(customConfigSettingsName));
      
      var xd = new XmlDocument();
      xd.Load(customConfigSettingsName);
      
      try
      {
        foreach (XmlNode node in xd.DocumentElement.ChildNodes)
        {
          if (node.Attributes["name"].Value == elementName)
            return node.Attributes["value"].Value;
        }
      }
      catch (Exception ex)
      {
        Logger.DebugFormat("Дополнительный конфигурационный файл имеет некорректный формат", ex);
      }

      throw new ArgumentException(MailAdapter.Resources.ConfigurationElementValueNotSpecifiedFormat(elementName));
    }
    
    /// <summary>
    /// Получить список гиперссылок для выполнения задания.
    /// </summary>
    /// <param name="assignment">Задание.</param>
    /// <param name="htmlMailBody">Признак того, что почтовый клиент требует html разметку тела ответного письма.</param>
    /// <returns>Список гиперссылок.</returns>
    private List<Structures.Module.HyperlinkExecuteAssigment> GetHyperlinkResults(IAssignmentBase assignment, bool? htmlMailBody)
    {
      // Ссылки на выполнения задания.
      var hyperlinkResults = new List<Structures.Module.HyperlinkExecuteAssigment>();
      
      var mailProcessAssignmentType = GetMailProcessAssignmentType(assignment);
      
      if (mailProcessAssignmentType == null)
        return hyperlinkResults;
      
      Sungero.Metadata.EntityMetadata meta = Sungero.Domain.Shared.TypeExtension.GetEntityMetadata(assignment);
      var isFirstRecord = true;
      foreach (var res in mailProcessAssignmentType.AllowResults)
      {
        var body = GetReplyBody(assignment, res.ReplyInstructionText, res.Result, htmlMailBody);
        var localizedName = Sungero.Domain.Shared.ResourceExtensions.GetLocalizedName(meta.Actions.Single(a => a.Name == res.Result));
        var subject = localizedName + ": " + assignment.Subject;
        var hyperLink = string.Format(Constants.Module.MailtoTemplate, ReplyEmailAdress(), subject, body).ToString();
        var color = string.Empty;
        if (res.IsCustomActionResult)
          color = string.IsNullOrEmpty(res.Color) ? Constants.Module.DarkBlueColor : res.Color;
        else
          color = isFirstRecord ? Constants.Module.GreenColor : Constants.Module.OrangeColor;
        var item = Structures.Module.HyperlinkExecuteAssigment.Create(localizedName, hyperLink, color);
        hyperlinkResults.Add((Structures.Module.HyperlinkExecuteAssigment)item);
        isFirstRecord = false;
      }
      
      return hyperlinkResults;
    }
    
    /// <summary>
    /// Получить тело ответного письма.
    /// </summary>
    /// <param name="assignment">Задание.</param>
    /// <param name="caption">Описание ответного письма.</param>
    /// <param name="selectedResult">Результат, с которым будет выполнено задание.</param>
    /// <param name="htmlMailBody">Признак того, что почтовый клиент требует html разметку тела ответного письма.</param>
    /// <returns>Тело ответного письма.</returns>
    private string GetReplyBody(IAssignmentBase assignment, string caption, string selectedResult, bool? htmlMailBody)
    {
      var result = new StringBuilder();
      // Сформировать служебную информацию о результате выполнения задания.
      var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(string.Format(Constants.Module.MailAssignmentResultTemplate, assignment.Id, selectedResult));
      var encodeString = System.Convert.ToBase64String(plainTextBytes);
      
      var separator = (htmlMailBody ?? false) ? Constants.Module.NewLineHtmlTag : string.Empty;
      var newLine = System.Uri.EscapeDataString("\n");
      
      result.Append(separator + separator + newLine);
      result.Append(Constants.Module.MailBodySeparator).Append(separator + newLine);
      result.Append(caption).Append(separator + newLine).Append(separator + newLine);
      result.Append(Resources.ServiceInformationText).Append(separator + newLine);
      result.Append(encodeString).Append(separator + newLine);
      result.Append(Constants.Module.MailBodySeparator).Append(separator + newLine);
      
      return result.ToString();
    }
    
    /// <summary>
    /// Обработать текст, выделив в нём отдельные абзацы и гиперссылки.
    /// </summary>
    /// <param name="context">Контекст письма.</param>
    /// <param name="args">Аргументы.</param>
    /// <param name="options">Опции.</param>
    /// <param name="function">Функция.</param>
    /// <param name="inverse">Инверс.</param>
    private void ProcessText(RenderContext context, IList<object> args, IDictionary<string, object> options, RenderBlock function, RenderBlock inverse)
    {
      // CORE: использование System.Environment.
      var text = (args[0] ?? string.Empty).ToString().Replace(Environment.NewLine, "\n");
      // CORE: использование new.
      var entityHyperlinksParser = new EntityHyperlinkParser(Sungero.Domain.Shared.HyperlinkParsers.HttpHyperlinkParser);
      var textChunks = entityHyperlinksParser.Parse(text);
      foreach (var chunk in textChunks)
        function(chunk);
    }
    
    /// <summary>
    /// Получить дату последней рассылки уведомлений.
    /// </summary>
    /// <returns>Дата последней рассылки.</returns>
    public static DateTime GetLastNotificationDate()
    {
      // CORE: строковая константа.
      var key = "LastNotificationOfAssignment";
      var command = string.Format(Queries.Module.SelectDocflowParamsValue, key);
      try
      {

        var date = Sungero.Docflow.PublicFunctions.Module.ExecuteScalarSQLCommand(command);
        Logger.DebugFormat("Last notification date in DB is {0} (UTC)", date);
        // CORE: использование System.Globalization.DateTimeStyles.
        DateTime result = Calendar.FromUtcTime(DateTime.Parse(date.ToString(), null, System.Globalization.DateTimeStyles.AdjustToUniversal));

        if ((result - Calendar.Now).TotalDays > 1)
          return Calendar.Today;
        else
          return result;

      }
      catch (Exception ex)
      {
        Logger.Error("Error while getting last notification date", ex);
        return Calendar.Today;
      }
    }
    
    /// <summary>
    /// Обновить дату последней рассылки уведомлений.
    /// </summary>
    /// <param name="notificationDate">Дата рассылки уведомлений.</param>
    [Public]
    public static void UpdateLastNotificationDate(DateTime notificationDate)
    {
      // CORE: строковая константа.
      var key = "LastNotificationOfAssignment";
      
      // CORE: SQL-запрос: Обновление параметра даты последней рассылки уведомлений.
      var newDate = notificationDate.Add(-Calendar.UtcOffset).ToString("yyyy-MM-ddTHH:mm:ss.ffff+0");
      
      Sungero.Docflow.PublicFunctions.Module.ExecuteSQLCommandFormat(Queries.Module.InsertOrUpdateDocflowParamsValue, new[] { key, newDate });
      
      Logger.DebugFormat("Last notification date is set to {0} (UTC)", newDate);
    }
    #endregion
    
    #region Обработка заданий в очереди
    
    /// <summary>
    /// Добавить задание в очередь на выполнение.
    /// </summary>
    /// <param name="item">Результат выполнения задания.</param>
    [Remote]
    public void AddAssignmentToProcessQueue(IAssignmentQueue item)
    {
      // Поставить в очередь асинхронное событие.
      var handler = AsyncHandlers.ProcessAssignment.Create();
      handler.ID = item.Id;
      handler.ExecuteAsync();
    }
    
    #endregion
    
    /// <summary>
    /// Получить элемент очереди выполнения заданий через почту.
    /// </summary>
    /// <param name="assignmentResultString">Строкое представление результата выполнения задания.</param>
    /// <param name="activeText">Комментарий выполнения задания.</param>
    /// <param name="mailInfo">Информация о письме для записи в лог.</param>
    /// <returns>Запись справочника с ИД задания и результатом выполнения задания.</returns>
    [Public, Remote]
    public IAssignmentQueue GetAssignmentResult(string assignmentResultString, string activeText, string mailInfo)
    {
      var result = AssignmentQueues.Create();
      var math = Regex.Match(assignmentResultString, "AssignmentId=([0-9]*);result=([a-z]*)", RegexOptions.IgnoreCase);
      result.ActiveText = activeText;
      result.MailInfo = mailInfo;
      
      if (math.Groups.Count == 3)
      {
        var assignmentIdString = math.Groups[1].Value;
        result.Result = math.Groups[2].Value;
        int i;
        var assignmentId = int.TryParse(assignmentIdString, out i) ? (int?)i : (int?)null;
        if (assignmentId != null)
          result.Assignment = Assignments.GetAll(x => x.Id == assignmentId && x.Status == Sungero.Workflow.AssignmentBase.Status.InProcess).FirstOrDefault();
      }
      return result;
    }
    
    /// <summary>
    /// Вложить документы в задание.
    /// </summary>
    /// <param name="item">Элемент очереди выполнения заданий.</param>
    public virtual void CreateAttachments(IAssignmentQueue item)
    {
      if (item.Assignment == null)
        return;
      
      foreach (var row in item.Attachments.Where(f => f.FileDescription.ToUpper() != Constants.Module.FileBodyTXT && f.FileDescription.ToUpper() != Constants.Module.FileBodyHTML))
      {
        Logger.DebugFormat("Create attachment: {0}", row.FileDescription);
        var ext = Path.GetExtension(row.FileDescription).Replace(".", string.Empty);
        var app = Sungero.Content.AssociatedApplications.GetAll(a => a.Extension == ext).FirstOrDefault();
        if (app == null)
          continue;
        
        var document = SimpleDocuments.Create();
        
        // Вложить простой документ в задание.
        using (var stream = new System.IO.MemoryStream())
        {
          stream.Write(row.File, 0, row.File.Length);
          document.CreateVersionFrom(stream, ext);
          document.Name = Path.GetFileNameWithoutExtension(row.FileDescription);
          document.Save();
        }
        document.AccessRights.Grant(item.Assignment.Performer, DefaultAccessRightsTypes.FullAccess);
        document.AccessRights.Grant(item.Assignment.Author, DefaultAccessRightsTypes.Change);
        document.AccessRights.Save();
        item.Assignment.Attachments.Add(document);
      }
      item.Assignment.Save();
    }
    
    /// <summary>
    /// Удалить задание из очереди.
    /// </summary>
    /// <param name="item">Элемент очереди.</param>
    [Remote]
    public void RemoveAssignmentFromQueue(IAssignmentQueue item)
    {
      AssignmentQueues.Delete(item);
    }
    
    #region Подписание документов
    
    /// <summary>
    /// Подписать вложенные документы, если задание на согласование.
    /// </summary>
    /// <param name="item">Очередь выполнения заданий.</param>
    public virtual void ApproveAttachedDocuments(IAssignmentQueue item)
    {
      if (FreeApprovalAssignments.Is(item.Assignment) || ApprovalAssignments.Is(item.Assignment))
      {
        var docs = GetDocumentsForEndorse(item.Assignment);
        EndorseDocuments(item.Assignment, docs, item.Result, item.ActiveText);
      }
    }
    
    /// <summary>
    /// Возвратить список документов, которые необходимо подписать.
    /// </summary>
    /// <param name="assignment">Задание.</param>
    /// <returns>Список документов, которые необходимо подписать.</returns>
    [Public]
    public virtual List<Sungero.Content.IElectronicDocument> GetDocumentsForEndorse(IAssignment assignment)
    {
      var result = new List<Sungero.Content.IElectronicDocument>();
      // Задание на свободное согласование.
      if (FreeApprovalAssignments.Is(assignment))
      {
        var freeApprovalAssignment = FreeApprovalAssignments.As(assignment);
        var task = FreeApprovalTasks.As(freeApprovalAssignment.Task);
        // На согласовании всегда 1 документ.
        result.Add(task.ForApprovalGroup.ElectronicDocuments.First());
        // Добавить приложения.
        foreach (var addendum in task.AddendaGroup.ElectronicDocuments)
          result.Add(addendum);
      }
      // Согласование по регламенту.
      if (ApprovalAssignments.Is(assignment))
      {
        var approvalAssignment = ApprovalAssignments.As(assignment);
        var task = Sungero.Docflow.ApprovalTasks.As(assignment.Task);
        // На согласовании всегда 1 документ.
        result.Add(task.DocumentGroup.OfficialDocuments.First());
        // Добавить приложения.
        foreach (var item in task.AddendaGroup.OfficialDocuments)
          result.Add(item);
      }
      return result;
    }
    
    /// <summary>
    /// Подписать документы.
    /// </summary>
    /// <param name="assignment">Выполненное задание.</param>
    /// <param name="documents">Список документов.</param>
    /// <param name="assignmentResult">Результат выполнения задания.</param>
    /// <param name="activeText">Комментарий к выполнению задания.</param>
    /// <returns>Признак того, что удалось ли подписать все вложенные файлы.</returns>
    public bool EndorseDocuments(Sungero.Workflow.IAssignment assignment, List<Sungero.Content.IElectronicDocument> documents, string assignmentResult, string activeText)
    {
      var result = true;
      documents = documents.Where(x => x.HasVersions).ToList();
      if (documents.Any())
      {
        var performer = assignment.Performer;
        foreach (var doc in documents)
        {
          // Выдать права на чтение системному пользователю.
          doc.AccessRights.Grant(Users.Current, DefaultAccessRightsTypes.Read);
          doc.AccessRights.Save();
          // Подписать согласующей подписью.
          if (assignmentResult == Sungero.Docflow.FreeApprovalAssignment.Result.Approved.Value || assignmentResult == Sungero.Docflow.ApprovalAssignment.Result.Approved.Value)
          {
            var isEndorse = Signatures.Endorse(doc.LastVersion, null, activeText, performer);
            result = result && isEndorse;
          }
          // Подписать с результатом "Не согласовано".
          if (assignmentResult == Sungero.Docflow.FreeApprovalAssignment.Result.ForRework.Value || assignmentResult == Sungero.Docflow.ApprovalAssignment.Result.ForRevision.Value)
          {
            var isNotEndorse = Signatures.NotEndorse(doc.LastVersion, null, activeText, performer);
            result = result && isNotEndorse;
          }
        }
      }
      return result;
    }
    
    #endregion
    
    #region Обработка прикладных действий (функций)
    
    /// <summary>
    /// Получить список информации по прикладным действиям.
    /// </summary>
    /// <returns>Список информации по прикладным действиям, использующимся при отправке писем с заданиями.</returns>
    public virtual List<Structures.Module.CustomActionInformation> GetCustomActionForSend()
    {
      return new List<Structures.Module.CustomActionInformation>();
    }
    
    /// <summary>
    /// Добавить в список доступных результатов прикладные действия.
    /// </summary>
    /// <param name="assignmentTypeName">Тип задания.</param>
    /// <param name="results">Список доступных результатов.</param>
    private void AddCustomActionResults(string assignmentTypeName, List<Structures.Module.AllowResultData> results)
    {
      var actions = GetCustomActionForSend().Where(x => x.EntityTypeName == assignmentTypeName);
      foreach (var element in actions)
        results.Add(Structures.Module.AllowResultData.Create(element.ActionName, Resources.ReplyInstructionTextCustomActionComplete, true, element.ButtonColor));
    }
    
    #endregion

    #region Обработка DCTS
    
    /// <summary>
    /// Обработать пакеты службы ввода DCTS.
    /// </summary>
    /// <param name="lineSender">Наименование линии.</param>
    /// <param name="instanceInfos">Путь к xml файлу DCTS c информацией об экземплярах захвата и о захваченных файлах.</param>
    /// <param name="deviceInfo">Путь к xml файлу DCTS c информацией об устройствах ввода.</param>
    /// <param name="inputFiles">Путь к xml файлу DCTS c информацией об отправляемых в конечную систему файлах.</param>
    /// <param name="filesBase64">Список файлов в формате Base64 переданных DCS</param>
    /// <param name="filesName">Список наименований файлов в формате Base64 переданных DCS</param>
    [Public(WebApiRequestType = RequestType.Post)]
    public static void ProcessingDCTS(string lineSender, string instanceInfos, string deviceInfo, string inputFiles, List<string> filesBase64, List<string> filesName)
    {
      Functions.Module.InternalProcessingDCTS(lineSender, instanceInfos, deviceInfo, inputFiles, filesBase64, filesName);
    }
    
    /// <summary>
    /// Обработать пакеты службы ввода DCTS.
    /// </summary>
    /// <param name="lineSender">Наименование линии.</param>
    /// <param name="instanceInfos">Путь к xml файлу DCTS c информацией об экземплярах захвата и о захваченных файлах.</param>
    /// <param name="deviceInfo">Путь к xml файлу DCTS c информацией об устройствах ввода.</param>
    /// <param name="inputFiles">Путь к xml файлу DCTS c информацией об отправляемых в конечную систему файлах.</param>
    /// <param name="filesBase64">Список файлов в формате Base64 переданных DCS</param>
    /// <param name="filesName">Список наименований файлов в формате Base64 переданных DCS</param>
    public virtual void InternalProcessingDCTS(string lineSender, string instanceInfos, string deviceInfo, string inputFiles, List<string> filesBase64, List<string> filesName)
    {
      var files = new List<Structures.Module.IDocumentPackage>();
      var numberFile = 0;
      foreach (var file in filesName)
      {
        var document = Structures.Module.DocumentPackage.Create();
        document.FileName = file;
        document.FileBase64 = filesBase64.ElementAt(numberFile);
        files.Add(document);
        numberFile++;
      }
      // Получить задание, которое удовлетворяет критериям.
      var assignmentResult = this.GetAssignmentResultFromMail(instanceInfos, deviceInfo, inputFiles, files);
      if (assignmentResult.Assignment != null)
      {
        // Вложить файлы из письма для простой задачи.
        if (SimpleAssignments.Is(assignmentResult.Assignment))
        {
          var inputFilesDocument = this.GetXDocumentFromString(inputFiles);
          foreach (var el in inputFilesDocument.Element("InputFilesSection").Element("Files").Elements())
          {
            var row = assignmentResult.Attachments.AddNew();
            row.File = Convert.FromBase64String(files.FirstOrDefault(f => f.FileName.Contains(el.Element("FileName").Value)).FileBase64);
            row.FileDescription = el.Element("FileDescription").Value;
          }
        }
        assignmentResult.Save();
        Functions.Module.AddAssignmentToProcessQueue(assignmentResult);
      }
      else if (assignmentResult.Assignment == null)
        Logger.ErrorFormat(MailAdapter.Resources.IncomingMailProcessedErrorTemplate, assignmentResult.MailInfo);
      else
        Logger.ErrorFormat(MailAdapter.Resources.BadMailBodyTemplate, assignmentResult.MailInfo);
    }
    
    /// <summary>
    /// Получить элемент очереди выполнения заданий через почту.
    /// </summary>
    /// <param name="instanceInfos">Информация об экземплярах захвата.</param>
    /// <param name="deviceInfo">Информация об устройствах ввода.</param>
    /// <param name="inputFiles">Информация о захваченных файлах.</param>
    /// <param name="folder">Папка хранения файлов.</param>
    /// <returns>Структура с ИД задания и результатом выполнения задания.</returns>
    public IAssignmentQueue GetAssignmentResultFromMail(string instanceInfos, string deviceInfo, string inputFiles, List<Structures.Module.IDocumentPackage> files)
    {
      var deviceInfoXDoc = this.GetXDocumentFromString(deviceInfo);
      // Модуль ввода, с которого пришел пакет.
      var captureServiceXElement = deviceInfoXDoc.FirstNode as XElement;
      if (captureServiceXElement.Name.LocalName != "MailSourceInfo")
        throw new InvalidOperationException(Resources.DctsDeviceTypeIncorrect);
      
      // Получить из XML информацию о письме.
      var instanceInfoDocument = this.GetXDocumentFromString(instanceInfos);
      var mailCaptureInstanceInfoElement = instanceInfoDocument.Elements().First().Element("MailCaptureInstanceInfo");
      var mailAddress = mailCaptureInstanceInfoElement.Element("From").Attribute("Address").Value;
      var mailDate = mailCaptureInstanceInfoElement.Element("Create").Value;
      var subjectAttribute = mailCaptureInstanceInfoElement.Attribute("Subject");
      var mailSubject = subjectAttribute != null ? subjectAttribute.Value : string.Empty;
      if (mailSubject.Length > 250)
        mailSubject = mailSubject.Substring(0, 249);
      var mailInfo  = string.Format(MailAdapter.Resources.MailInfoTemplate, mailAddress, mailDate, mailSubject);
      
      var inputFilesDocument = this.GetXDocumentFromString(inputFiles);
      var body = this.GetBodyMail(inputFilesDocument, files);
      var activeText = MailAdapter.Functions.Module.FilterActiveText(body, mailAddress);
      var assignmentResultString = string.Empty;
      try
      {
        assignmentResultString = this.DecodeAssignmentResult(body);
      }
      catch (Exception)
      {
        Logger.ErrorFormat(MailAdapter.Resources.BadMailBodyTemplate, mailInfo);
      }
      return Functions.Module.GetAssignmentResult(assignmentResultString, activeText, mailInfo);
    }
    
    /// <summary>
    /// Декодировать результат выполнения задания из тела письма.
    /// </summary>
    /// <param name="body">Тело письма.</param>
    /// <returns>Декодированная строка с результатом выполнения задания.</returns>
    private string DecodeAssignmentResult(string body)
    {
      // Получить строку между разделителями. В данной строке закодирована служебная информация, о результате выполнения задания.
      var serviceInfo = body.Split(new[] { Constants.Module.MailBodySeparator.ToString() }, StringSplitOptions.None)[1];
      var lastString = serviceInfo.Split(new[] { System.Environment.NewLine }, StringSplitOptions.None).LastOrDefault(x => !string.IsNullOrWhiteSpace(x));
      
      var regex = new Regex(Constants.Module.NewLineHtmlTag, RegexOptions.IgnoreCase);
      lastString = regex.Replace(lastString, string.Empty);
      
      var base64EncodedBytes = System.Convert.FromBase64String(lastString);
      return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
    
    /// <summary>
    /// Получить XDocument из xml файла.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <returns>XDocument документ.</returns>
    private XDocument GetXDocumentFromFile(string path)
    {
      if (!File.Exists(path)) throw new FileNotFoundException(string.Format(Resources.FileNotFoundTemplate, path));
      return XDocument.Load(path);
    }
    
    /// <summary>
    /// Получает XDocument из xml строки.
    /// </summary>
    /// <param name="path">xml строка.</param>
    /// <returns>XElement документ.</returns>
    private XDocument GetXDocumentFromString(string xmlString)
    {
      return XDocument.Parse(xmlString);
    }
    
    /// <summary>
    /// Получить тело письма.
    /// </summary>
    /// <param name="inputFilesDocument">Файл с метаданными.</param>
    /// <param name="folder">Папка с файлами.</param>
    /// <returns>Тело письма.</returns>
    private string GetBodyMail(XDocument inputFilesDocument, List<Structures.Module.IDocumentPackage> files)
    {
      // Берем первый элемент FileInfo, т.к. первый элемент всегда тело письма.
      var fileInfo = inputFilesDocument.Element("InputFilesSection").Element("Files").Elements("FileInfo").FirstOrDefault();
      // Кодировка тела письма в юникоде.
      var file = files.Where(f => f.FileName.Contains(fileInfo.Element("FileName").Value)).FirstOrDefault();
      //var textBytes = Convert.FromBase64String(Convert.ToBase64String(file.FileBase64));
      var bytes = Convert.FromBase64String(file.FileBase64);
      var text = System.Text.Encoding.Unicode.GetString(bytes);
      // HACK. Если тело письма - HTML.
      if (Path.GetExtension(file.FileName).ToUpper() == ".HTML")
        text = GetPlainTextFromHtml(text);
      
      return text;
    }
    
    /// <summary>
    /// Получить содержимое письма из HTML.
    /// </summary>
    /// <param name="htmlString">HTML письма.</param>
    /// <returns>Plain text письма.</returns>
    private static string GetPlainTextFromHtml(string htmlString)
    {
      string htmlTagPattern = "<.*?>";
      var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
      htmlString = regexCss.Replace(htmlString, string.Empty);
      htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
      htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
      htmlString = htmlString.Replace("&nbsp;", string.Empty);

      return htmlString;
    }
    
    #endregion
    
  }
}