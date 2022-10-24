using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;

namespace DirRX.MailAdapter.Structures.Module
{

  /// <summary>
  /// Файлы принятые от DCS
  /// </summary>
  [Public]
  partial class DocumentPackage
  {
    /// <summary>
    /// Документ в формате Base64.
    /// </summary>
    public string FileBase64 { get; set; }
    
    /// <summary>
    /// Наименование файла.
    /// </summary>
    public string FileName { get; set; }
  }

  /// <summary>
  /// Структура информации по прикладному действию.
  /// </summary>
  partial class CustomActionInformation
  {
    /// <summary>
    /// Имя типа сущности.
    /// </summary>
    public string EntityTypeName { get; set; }
    
    /// <summary>
    /// Имя действия.
    /// </summary>
    public string ActionName { get; set; }
    
    /// <summary>
    /// Имя серверной функции обернутой в действие.
    /// </summary>
    public string ServerFunctionName { get; set; }
    
    /// <summary>
    ///  Цвет гиперссылки(кнопки) в формате HEX.
    /// </summary>
    /// <remarks>Свойство не обязатьльное, если не заполнено берется цвет по умолчанию.</remarks>
    public string ButtonColor { get; set; }
  }

  /// <summary>
  /// Очередь заданий.
  /// </summary>
  partial class AssignmentQueue
  {
    public List<DirRX.MailAdapter.Structures.Module.AssignmentQueueItem> QueueItems { get; set; }
  }
  
  /// <summary>
  /// Элемент очереди заданий на выполнение.
  /// </summary>
  partial class AssignmentQueueItem
  {
    /// <summary>
    /// ИД задания.
    /// </summary>
    public int AssignmentId { get; set; }
    
    /// <summary>
    /// Результат выполнения задания.
    /// </summary>
    public string Result { get; set; }
    
    /// <summary>
    /// Комментарий к выполнению задания.
    /// </summary>
    public string ActiveText { get; set; }
    
    /// <summary>
    /// Информация о письме для записи в лог.
    /// </summary>
    public string MailInfo { get; set; }
    
    /// <summary>
    /// Список вложений.
    /// </summary>
    public List<int> Attachments { get; set; }
  }

  /// <summary>
  /// Результат отправки писем.
  /// </summary>
  [Public]
  partial class MailSendResult
  {
    /// <summary>
    /// Признак того, что при отправке не было ошибок.
    /// </summary>
    public bool IsSendMailSuccess { get; set; }

    /// <summary>
    /// Признак того, что было отправлено хотя бы одно письмо.
    /// </summary>
    public bool IsAnyMailSended { get; set; }
  }
  
  /// <summary>
  /// Структура ссылок для выполнения задания.
  /// </summary>
  [Public]
  partial class HyperlinkExecuteAssigment
  {
    /// <summary>
    /// Отображаемое значение.
    /// </summary>
    public string DisplayValue { get; set; }
    
    /// <summary>
    /// Ссылка для выполнения задания.
    /// </summary>
    public string HyperLinkItem { get; set; }
    
    /// <summary>
    /// Цвет заполнения кнопки с гиперссылкой.
    /// </summary>
    public string FillColor { get; set; }
  }
  
  /// <summary>
  /// Результат выполнения задания.
  /// </summary>
  [Public]
  partial class AssignmentResult
  {
    /// <summary>
    /// Задание.
    /// </summary>
    public int? AssignmentId { get; set; }
    
    /// <summary>
    /// Результат выполнения задания.
    /// </summary>
    public string Result { get; set; }
    
    /// <summary>
    /// Комментарий к выполнению задания.
    /// </summary>
    public string ActiveText { get; set; }
    
    /// <summary>
    /// Информация о письме для записи в лог.
    /// </summary>
    public string MailInfo { get; set; }
  }
  
  /// <summary>
  /// Структура для хранения описания файла в DCTS.
  /// </summary>
  partial class FileInfo
  {
    /// <summary>
    /// Путь к файлу.
    /// </summary>
    public string FileName { get; set; }
    
    /// <summary>
    /// Наименование файла.
    /// </summary>
    public string FileDescription { get; set; }
  }

  /// <summary>
  /// Структура с результатом выполнения задания и сопроводительным текстом ответного письма.
  /// </summary>
  partial class AllowResultData
  {
    /// <summary>
    /// Результат выполнения задания.
    /// </summary>
    public string Result { get; set; }
    
    /// <summary>
    /// Текст ответного письма.
    /// </summary>
    public string ReplyInstructionText { get; set; }
    
    /// <summary>
    /// Признак результата прикладного действия.
    /// </summary>
    public bool IsCustomActionResult { get; set; }
        
    /// <summary>
    /// Цвет кнопки.
    /// </summary>
    public string Color { get; set; }
  }
  
  /// <summary>
  /// Структура для хранения типа задания, выполнимого через почту.
  /// </summary>
  partial class MailProcessAssignmentType
  {
    /// <summary>
    /// Тип задания.
    /// </summary>
    public string AssignmentType { get; set; }
    
    /// <summary>
    /// Текст письма.
    /// </summary>
    public string IncomingInstructionText { get; set; }
    
    /// <summary>
    /// Результаты выполнения задания с учетом порядка раположения кнопок выполнения данного задания.
    /// </summary>
    public List<DirRX.MailAdapter.Structures.Module.AllowResultData> AllowResults { get; set; }
  }
}