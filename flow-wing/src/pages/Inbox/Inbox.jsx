import React, { useEffect, useState } from "react";
import "./inbox.css";
import { Icon } from "@iconify/react";
import { Tooltip } from "@mui/material";
import { Divider } from "antd";
import EmailService from "../../services/emailService";
import { useParams } from "react-router";

const Inbox = () => {
  const [mail, setMail] = useState(null);

  let { id } = useParams();
  let emailService = new EmailService();

  useEffect(() => {
    emailService.getEmailById(id).then((res) => setMail(res.data));
  }, [id]);

  if (!mail) {
    return <div>Loading...</div>;
  }

  // sentDateTime'ı tarih ve saat olarak ayır
  const sentDateTime = new Date(mail.sentDateTime);
  const formattedDate = sentDateTime.toLocaleDateString("tr-TR");
  const formattedTime = sentDateTime.toLocaleTimeString("tr-TR");

  return (
    <div className="inbox-page-content">
      <div className="mail-actions">
        <Tooltip title="İlet" arrow>
          <div className="icons">
            <button className="mail-action-btns">
              <Icon
                icon="material-symbols-light:forward"
                width="40"
                rotate={2}
              />
            </button>
          </div>
        </Tooltip>
        <Tooltip title="Yanıtla" arrow>
          <div className="icons">
            <button className="mail-action-btns">
              <Icon icon="iconoir:reply" width="30" />
            </button>
          </div>{" "}
        </Tooltip>
        <Tooltip title="Sil" arrow>
          <div className="icons">
            <button className="mail-action-btns">
              <Icon icon="bi:trash" width="30" />
            </button>
          </div>
        </Tooltip>
      </div>
      <div className="mail-sender">
        <div className="user-icon-home">
          <Icon icon="ph:user-light" width="70" />
        </div>
        <div>
          <div className="icon-with-text">
            <Icon icon="bi:ticket-fill" color="#b31312" width="30" />
            <span>{mail.user}</span>
          </div>

          <div className="mail-sender-email">{mail.senderEmail}</div>
        </div>
      </div>

      <div className="mail-title">
        <Icon icon="uit:subject" color="#b31312" width="40" />{" "}
        <h3>{mail.emailSubject}</h3>
      </div>
      <p>{mail.sentEmailBody}</p>

      <div className="mail-attachments">
        <div className="attachment-content">
          <Icon
            icon="material-symbols-light:attachment"
            color="#b31312"
            width="30"
          />
          <span>{mail.attachment}</span>
        </div>
        <Divider />
      </div>

      <div className="inbox-mail-summary">
        <p>
          <span>Kimden:</span> {mail.senderEmail}
        </p>
        <p>
          <span>Konu:</span> {mail.emailSubject}
        </p>
        <p>
          <span>Gönderilme Tarihi:</span> {formattedTime} - {formattedDate} 
        </p>
      </div>
    </div>
  );
};

export default Inbox;
