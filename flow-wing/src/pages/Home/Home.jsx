import React, { useEffect, useState } from "react";
import "./home.css";
import data from "../../data.json";
import { Icon } from "@iconify/react";
import { Link } from "react-router-dom";
import Divider from "@mui/material/Divider";
import EmailService from "../../services/emailService";
import alertify from "alertifyjs";

const Home = () => {
  const [mails, setMails] = useState([]);
  let emailService = new EmailService();
  // to shorten the mail body
  const excerpt = (str, count) => {
    if (str && str.length > count) {
      str = str.substring(0, count) + "...";
    }
    return str;
  };
  // Get all e-mails
  useEffect(() => {
    const fetchMails = async () => {
      try {
        const response = await emailService.getMails();
        const sortedMails = response.data.sort((a, b) => new Date(b.sentDateTime) - new Date(a.sentDateTime));
        setMails(sortedMails);
      } catch (error) {
        console.error("Error fetching mails:", error);
      }
    };

    fetchMails();
  }, []);


  // COLOR ARRAY FOR HR ELEMENT
  const colors = ["#C0440E", "#3498db", "#27ae60", "#f39c12", "#8e44ad"]; // İstediğiniz renkleri ekleyin
  return (
    <div className="home-page-content">
      <h2>Gelen Mailler</h2>
      <div className="inbox">
        {mails.map((item, index) => {
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
            <Link to={`/inbox/${index}`}>
              <div className="mail-content">
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

                    <div className="user-name">{item.senderEmail} </div>
                  </div>
                  <div className="inbox-mail-title">{item.emailSubject}</div>
                  <div className="inbox-mail-body">
                    {excerpt(item.sentEmailBody, 120)}
                  </div>
                </div>{" "}
                <div className="inbox-sent-time">{timeToShow}</div>
                <Divider />
              </div>
            </Link>
          );
        })}
      </div>
    </div>
  );
};

export default Home;