import React, { useEffect, useState } from "react";
import { Icon } from "@iconify/react";
import { Link, useNavigate, useParams } from "react-router-dom";
import Divider from "@mui/material/Divider";
import EmailService from "../../services/emailService";
import "./sent.css";

const Sent = () => {
  const [sentMails, setSentMails] = useState([]);
  let navigate = useNavigate()
  let { id } = useParams();
  // to shorten the mail body
  const excerpt = (str, count) => {
    if (str && str.length > count) {
      str = str.substring(0, count) + "...";
    }
    return str;
  };

  const emailService = new EmailService();

  // get sent emails
  useEffect(() => {
    emailService.getSentMails().then((res) => {
      // Sort the sentMails array by sentDateTime in descending order
      const sortedMails = res.data.userEmails.sort(
        (a, b) => new Date(b.sentDateTime) - new Date(a.sentDateTime)
      );
      setSentMails(sortedMails);
    });
  }, []);

  // COLOR ARRAY FOR HR ELEMENT
  const colors = ["#C0440E", "#3498db", "#27ae60", "#f39c12", "#8e44ad"]; // İstediğiniz renkleri ekleyin

  // delete email
  const handleDelete = (id) => {
    emailService.deleteSentEmail(id).then((res) => {
      console.log(res);
      // Update the sentMails state after deleting the email
      setSentMails(sentMails.filter((mail) => mail.id !== id));
      navigate("/sent")
    });
  };
  return (
    <div className="sent-mail-page-content">
      <h2>Gönderilmiş Mailler</h2>
      <div className="sent">
        {sentMails.map((item, index) => {
          // to format date
          const dateFromAPI = new Date(item.sentDateTime);
          const nowsDate = new Date();

          let timeToShow;

          if (
            dateFromAPI.getFullYear() === nowsDate.getFullYear() &&
            dateFromAPI.getMonth() === nowsDate.getMonth() &&
            dateFromAPI.getDate() === nowsDate.getDate()
          ) {
            const hourPart = dateFromAPI.toLocaleTimeString("tr-TR", {
              hour: "numeric",
              minute: "numeric",
            });
            timeToShow = hourPart;
          } else {
            const datePart = dateFromAPI.toLocaleDateString("tr-TR");
            timeToShow = datePart;
          }

          return (
            <>
              <Link to={`/inbox/${item.id}`} key={index}>
                <div className="sent-mail-content">
                  <hr
                    style={{
                      border: `1px solid ${colors[index % colors.length]}`,
                    }}
                  />
                  <div key={index} className="inbox-mail-unopened">
                    <div className="user-section">
                      <div className="user-icon-home">
                        <Icon icon="ph:user-light" width="30" />{" "}
                      </div>
                      <div className="user-name">{item.recipientsEmail} </div>
                    </div>
                    <div className="inbox-mail-title">{item.emailSubject}</div>
                    <div className="inbox-mail-body">
                      {excerpt(item.sentEmailBody, 120)}
                    </div>
                  </div>{" "}
                  <div className="repeat-delete-sent-time-section">
                    <div className="is-repeating-icon">
                      {item.isScheduled === true && <Icon icon="bi:repeat" />}
                    </div>{" "}
                    <div className="delete-mail">
                      {" "}
                      <button
                        className="delete-mail-btn"
                        onClick={() => handleDelete(item.id)}
                      >
                        <Icon icon="iconoir:trash" />
                      </button>
                    </div>{" "}
                    <div className="inbox-sent-time">{timeToShow}</div>
                  </div>
               
                </div>{" "}   <Divider />{" "}
              </Link>
            </>
          );
        })}
      </div>
    </div>
  );
};

export default Sent;
