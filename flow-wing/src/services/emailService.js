import axios from "axios";

// Create an instance of Axios
const axiosInstance = axios.create({
  baseURL: "http://localhost:5232/api/", 
  mode: 'cors'
})
axiosInstance.interceptors.request.use(
  function (config) {
    const userData = localStorage.getItem("user");
    const userObject = JSON.parse(userData);
    const userToken = userObject.token;
    config.headers = {
      ...config.headers,
      Authorization: `Bearer ${userToken}`
    }
    // Do something before request is sent
    console.log("Request Interceptor - Request Config: ", config)
    return config
  },
  function (error) {
    // Do something with request error
    return Promise.reject(error)
  }
)

// Add a response interceptor
axiosInstance.interceptors.response.use(
  function (response) {
    // Do something with response data
    console.log("Response Interceptor - Response Data: ", response.data)
    return response
  },
  function (error) {
    // Do something with response error
    return Promise.reject(error)
  }
)
export default class EmailService {
  getMails() {
   return axiosInstance.get("EmailLogs/GetUserSentEmails")
  }

  sendMail(values) {

    const { recipientsEmail, emailSubject, emailBody } = values;

    // Convert values to strings if necessary
    const mailContent = {
      recipientsEmail: String(recipientsEmail),
      emailSubject: String(emailSubject),
      emailBody: String(emailBody),
    };

    return axiosInstance.post("EmailLogs", mailContent);
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
    }
    return axios.post("ScheduledEmails/CreateScheduledEmail", mailContent);
  }

  getSentMails() {
 
    return axiosInstance.get("EmailLogs/GetUserReceivedEmails");
  }

  getAllUsers() {

    return axiosInstance.get("Users");
  }

  deleteSentEmail(id) {
   
    return axiosInstance.delete("EmailLogs/" + id);
  }

  getEmailById(id) {
    return axiosInstance.get("EmailLogs/" + id);
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
      repeatEndDate:repeatEndDate
    }
    return axiosInstance.post(
      "ScheduledEmails/CreateScheduledRepeatingEmail",mailContent
    );
  }
}
