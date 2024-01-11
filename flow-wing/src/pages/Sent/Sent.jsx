import React, { useEffect, useState } from "react";
import { Icon } from "@iconify/react";
import { Link, useParams } from "react-router-dom";
import Divider from "@mui/material/Divider";
import EmailService from "../../services/emailService";
import "./sent.css";

const Sent = () => {
  const [sentMails, setSentMails] = useState([]);
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
      const sortedMails = res.data.sort(
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
    });
  };
  return (
    <div className="sent-mail-page-content">
      <h2>Gönderilmiş Mailler</h2>
      <div className="sent">
        {sentMails.map((item, index) => {
          // to format date
          const gelenTarih = new Date(item.sentDateTime);
          const suAnkiTarih = new Date();

          let timeToShow;

          if (
            gelenTarih.getFullYear() === suAnkiTarih.getFullYear() &&
            gelenTarih.getMonth() === suAnkiTarih.getMonth() &&
            gelenTarih.getDate() === suAnkiTarih.getDate()
          ) {
            const saatKismi = gelenTarih.toLocaleTimeString("tr-TR", {
              hour: "numeric",
              minute: "numeric",
            });
            timeToShow = saatKismi;
          } else {
            const tarihKismi = gelenTarih.toLocaleDateString("tr-TR");
            timeToShow = tarihKismi;
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
                  <div>
                    {" "}
                    <div className="delete-mail">
                      {" "}
                      <button className="delete-mail-btn" onClick={() => handleDelete(item.id)}>
                        <Icon icon="iconoir:trash" />
                      </button>
                    </div>{" "}
                    <div className="inbox-sent-time">{timeToShow}</div>
                  </div>
                </div>{" "}
              </Link>
              <Divider />
            </>
          );
        })}
      </div>
    </div>
  );
};

export default Sent;
