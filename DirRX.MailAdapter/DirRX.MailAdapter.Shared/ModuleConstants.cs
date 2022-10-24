using System;

namespace DirRX.MailAdapter.Constants
{
  public static class Module
  {
    /// <summary>
    /// Максимальная длина полного имени файла, вложенного в письмо.
    /// </summary>
    public const int MailAttachmentFullNameMaxLenght = 260;

    /// <summary>
    /// Наименование типа задания SimpleAssignment.
    /// </summary>
    public const string SimpleAssignmentName = "SimpleAssignment";

    /// <summary>
    /// Наименование типа задания FreeApprovalAssignment.
    /// </summary>
    public const string FreeApprovalAssignmentName = "FreeApprovalAssignment";

    /// <summary>
    /// Наименование типа задания ApprovalAssignment.
    /// </summary>
    public const string ApprovalAssignmentName = "ApprovalAssignment";
    
    /// <summary>
    /// Наименование типа задания ActionItemExecutionAssignment.
    /// </summary>
    public const string ActionItemExecutionAssignmentName = "ActionItemExecutionAssignment";
    
    /// <summary>
    /// Наименование типа задания ActionItemSupervisorAssignment.
    /// </summary>
    public const string ActionItemSupervisorAssignmentName = "ActionItemSupervisorAssignment";

    /// <summary>
    /// Наименование типа задания ApprovalSendingAssignment.
    /// </summary>
    public const string ApprovalSendingAssignmentName = "ApprovalSendingAssignment";
    
    /// <summary>
    /// Наименование типа задания AcquaintanceAssignment.
    /// </summary>
    public const string AcquaintanceAssignmentName = "AcquaintanceAssignment";
    
    /// <summary>
    /// Наименование типа задания ApprovalSimpleAssignment.
    /// </summary>
    public const string ApprovalSimpleAssignmentName = "ApprovalSimpleAssignment";
    
    /// <summary>
    /// Наименование типа задания ApprovalCheckingAssignment.
    /// </summary>
    public const string ApprovalCheckingAssignmentName = "ApprovalCheckingAssignment";

    /// <summary>
    /// Цвет гиперссылки(кнопки) Green в формате HEX.
    /// </summary>
    public const string GreenColor = "#79A834";

    /// <summary>
    /// Цвет гиперссылки(кнопки) Orange в формате HEX.
    /// </summary>
    public const string OrangeColor = "#EF9C15";
    
    /// <summary>
    /// Цвет гиперссылки(кнопки) DarkBlue в формате HEX.
    /// </summary>
    public const string DarkBlueColor = "#00008B";

    /// <summary>
    /// Имя служебного файла DCTS, который не нужно вкладывать в задание.
    /// </summary>
    public const string FileBodyHTML = "BODY.HTML";

    /// <summary>
    /// Имя служебного файла DCTS, который не нужно вкладывать в задание.
    /// </summary>
    public const string FileBodyTXT = "BODY.TXT";

    /// <summary>
    /// Разделитель пользовательского текста и служебной информации в шаблоне ответного письма.
    /// </summary>
    public const string MailBodySeparator = "------------------------";

    /// <summary>
    /// Шаблон служебной строки ответного письма с результатом выполнения задания.
    /// </summary>
    public const string MailAssignmentResultTemplate = "AssignmentId={0};result={1}";

    /// <summary>
    /// Шаблон гиперссылки для выполнения задания.
    /// </summary>
    public const string MailtoTemplate = "mailto:{0}?subject={1}&body={2}";
    
    /// <summary>
    /// HTML тег переноса строки.
    /// </summary>
    public const string NewLineHtmlTag = "<br>";
    
    public static class AssignmentsQueue
    {
      /// <summary>
      /// Таблица для хранения очереди заданий на выполнение.
      /// </summary>
      public const string AssignmentsQueueTableName = "DirRX_MailAdapter_AssignmentsQueue";
      
      /// <summary>
      /// Таблица для хранения ИД вложений в задания на выполнении.
      /// </summary>
      public const string AssignmentAttachmentsTableName = "DirRX_MailAdapter_AssignmentsAttachments";
      
      public const string AssignmentIdColumnName = "Id";
      public const string ResultColumnName = "Result";
      public const string ActiveTextColumnName = "ActiveText";
      public const string MailInfoColumnName = "MailInfo";
    }
  }
}