import axios from "axios";

export default class EmailService {
  getMails() {
    const userData = localStorage.getItem("user");
    const userObject = JSON.parse(userData);
    const userToken = userObject.token;

    return axios.get("http://localhost:5232/api/EmailLogs/GetUserSentEmails", {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${userToken}`,
      },
      mode: "cors",
    });
  }

  sendMail(values) {
    const userData = localStorage.getItem("user");
    const userObject = JSON.parse(userData);
    const userToken = userObject.token;
    const { recipientsEmail, emailSubject, emailBody } = values;

    // Convert values to strings if necessary
    const mailContent = {
      recipientsEmail: String(recipientsEmail),
      emailSubject: String(emailSubject),
      emailBody: String(emailBody),
    };

    return axios.post("http://localhost:5232/api/EmailLogs", mailContent, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${userToken}`,
      },
      mode: "cors",
    });
  }

  sendScheduledMail() {
    const userData = localStorage.getItem("user");
    const userObject = JSON.parse(userData);
    const userToken = userObject.token;

    return axios.post("http://localhost:5232/api/ScheduledEmails", {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${userToken}`,
      },
      mode: "cors",
    });
  }

  getSentMails() {
    const userData = localStorage.getItem("user");
    const userObject = JSON.parse(userData);
    const userToken = userObject.token;
    return axios.get("http://localhost:5232/api/EmailLogs/GetUserReceivedEmails", {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${userToken}`,
      },
      mode: "cors",
    });
  }

  getAllUsers() {
    const userData = localStorage.getItem("user");
    const userObject = JSON.parse(userData);
    const userToken = userObject.token;

    return axios.get("http://localhost:5232/api/Users", {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${userToken}`,
      },
      mode: "cors",
    });
  }

  deleteSentEmail(id) {
    const userData = localStorage.getItem("user");
    const userObject = JSON.parse(userData);
    const userToken = userObject.token;
    return axios.delete("http://localhost:5232/api/EmailLogs/" + id, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${userToken}`,
      },
      mode: "cors",
    });
  }

  getEmailById(id) {
    const userData = localStorage.getItem("user");
    const userObject = JSON.parse(userData);
    const userToken = userObject.token;
    return axios.get("http://localhost:5232/api/EmailLogs/" + id, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${userToken}`,
      },
      mode: "cors",
    });
  }

  sendScheduledRepeatingMail(values) {
    const userData = localStorage.getItem("user");
    const userObject = JSON.parse(userData);
    const userToken = userObject.token;

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
      repeatEndDate:repeatEndDate
    }
    return axios.post(
      "http://localhost:5232/api/ScheduledEmails/CreateScheduledRepeatingEmail",mailContent,
      {
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${userToken}`,
        },
        mode: "cors",
      }
    );
  }
}
