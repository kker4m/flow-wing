import React, { useEffect, useState } from "react";
import "./home.css";
import { Icon } from "@iconify/react";
import { Link, useNavigate } from "react-router-dom";
import Divider from "@mui/material/Divider";
import EmailService from "../../services/emailService";
import alertify from "alertifyjs";
import EmptyPage from "../../components/EmptyPage";

const Home = () => {
  const [mails, setMails] = useState([]);
  let navigate = useNavigate()
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
        console.log("gelen mailler: ", response.data);

        const sortedMails = response.data.sort(
          (a, b) => new Date(b.sentDateTime) - new Date(a.sentDateTime)
        );

        setMails(sortedMails);
      } catch (error) {
        console.error("Error fetching mails:", error);
      }
    };

    fetchMails();
  }, []);

  // DELETE AN EMAIL
  const handleDelete = (id) => {
    emailService.deleteSentEmail(id).then((res) => {
      console.log(res);
      // Update the sentMails state after deleting the email
      setMails(mails.filter((mail) => mail.id !== id));
      alertify.success("Mail silindi.");
      navigate("/home")
    });
  };
  // COLOR ARRAY FOR HR ELEMENT
  const colors = ["#C0440E", "#3498db", "#27ae60", "#f39c12", "#8e44ad"]; // İstediğiniz renkleri ekleyin

  if (mails.length===0) {
    return <EmptyPage/>;
  }
  return (
    <div className="home-page-content">
      <h2>Gelen Mailler</h2>
      <div className="inbox">
        {mails.map((item, index) => {
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
                  
                  <div>
                    {" "}
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
                </div><Divider />
             </Link>
            </>
          );
        })}
      </div>
    </div>
  );
};

export default Home;
