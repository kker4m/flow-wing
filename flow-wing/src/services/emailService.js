import axios from "axios";
import apiAxios from "../lib/apiAxios";


export default class EmailService {
  getMails() {
   return apiAxios.get("EmailLogs/GetUserSentEmails")
  }

  sendMail(values) {

    const { recipientsEmail, emailSubject, emailBody } = values;

    // Convert values to strings if necessary
    const mailContent = {
      recipientsEmail: String(recipientsEmail),
      emailSubject: String(emailSubject),
      emailBody: String(emailBody),
      attachments:[]
    };

    return apiAxios.post("EmailLogs", mailContent);
  }

  sendScheduledMail(values) {
  

    const {sentDateTime,
      recipientsEmail,
      emailSubject,
      emailBody,
    } = values;

    const mailContent={
      sentDateTime:sentDateTime,
      recipientsEmail:recipientsEmail,
      emailSubject:emailSubject,
      emailBody:emailBody,
      attachments:[]
    }
    return apiAxios.post("ScheduledEmails/CreateScheduledEmail", mailContent);
  }

  getSentMails() {
 
    return apiAxios.get("EmailLogs/GetUserReceivedEmails");
  }

  getAllUsers() {

    return apiAxios.get("Users");
  }

  deleteSentEmail(id) {
   
    return apiAxios.delete("EmailLogs/" + id);
  }

  getEmailById(id) {
    return apiAxios.get("EmailLogs/" + id);
  }

  sendScheduledRepeatingMail(values) {
  

    const {
      recipientsEmail,
      emailSubject,
      emailBody,
      nextSendingDate,
      repeatInterval,
      repeatEndDate,
    } = values;

    const mailContent={
      recipientsEmail:recipientsEmail,
      emailSubject:emailSubject,
      emailBody:emailBody,
      nextSendingDate:nextSendingDate,
      repeatInterval:repeatInterval,
      repeatEndDate:repeatEndDate,
     
    }
    return apiAxios.post(
      "ScheduledEmails/CreateScheduledRepeatingEmail",mailContent
    );
  }
}
