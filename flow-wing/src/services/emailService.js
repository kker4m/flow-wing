import axios from "axios";

export default class EmailService {
  sendMail(values) {
    const { recipientsEmail, emailSubject, emailBody } = values;

    // Convert values to strings if necessary
    const mailContent = {
      recipientsEmail: String(recipientsEmail),
      emailSubject: String(emailSubject),
      emailBody: String(emailBody),
    };

    return axios.post("http://localhost:2255/api/EmailLogs", mailContent, {
      headers: {
        "Content-Type": "application/json",
      },
      mode: "cors",
    });
  }


  sendScheduledMail(){
    return axios.post("http://localhost:2255/api/ScheduledEmails")
  }
}
