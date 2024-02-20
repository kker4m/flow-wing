import React, { useEffect, useState } from "react"
import "./inbox.css"
import { Icon } from "@iconify/react"
import { Tooltip } from "@mui/material"
import { Divider } from "antd"
import { useNavigate, useParams } from "react-router"
import Spinner from "../../components/Spinner"
import alertify from "alertifyjs"
import { deleteSentEmail, getEmailById } from "../../services/emailService"

const Inbox = () => {
  const [mail, setMail] = useState(null)
  const [attachment, setAttachment] = useState("")
  const [sender, setSender] = useState(true)
  const [user, setUser] = useState("")
  let navigate = useNavigate()
  let { id } = useParams()

  useEffect(() => {
    getEmailById(id).then((res) => {
      setMail(res.data.emailLog)
      console.log("get mail by id", res.data)
      setAttachment(res.data.attachments[0])
      console.log("attachment: ", attachment)
      setSender(res.data.sender)
      console.log("mail sender", sender)
      setUser(res.data.emailLog.user.username)
      console.log("user", res.data.emailLog.user.username)
    })
    return () => {}
  }, [id])

  if (!mail) {
    return <Spinner />
  }
  // DELETE AN EMAIL
  const handleDelete = () => {
    deleteSentEmail(mail.id).then((res) => {
      console.log(res)
      alertify.success("Mail silindi.")
      navigate("/home")
    })
  }
  // sentDateTime'ı tarih ve saat olarak ayır
  const sentDateTime = new Date(mail.sentDateTime)
  const formattedDate = sentDateTime.toLocaleDateString("tr-TR")
  const formattedTime = sentDateTime.toLocaleTimeString("tr-TR")

  return sender === false ? (
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
            <button className="mail-action-btns" onClick={handleDelete}>
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
            <span>{user}</span>
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
        <Icon icon="ri:attachment-fill" width="20" height="20" />
        {attachment.fileName} -----
        <div>
          {attachment.contentType === "text/plain" && (
            <iframe
              title="Attachment"
              width="600"
              height="400"
              srcDoc={atob(attachment.data)}
            />
          )}
          {attachment.contentType === "application/pdf" && (
            <embed
              src={`data:application/pdf;base64,${attachment.data}`}
              type="application/pdf"
              width="500"
              height="500"
            />
          )}
          {["image/jpeg", "image/png", "image/gif"].includes(
            attachment.contentType
          ) && (
            <img
              src={`data:${attachment.contentType};base64,${attachment.data}`}
              alt="Attachment"
              width="100"
              height="100"
            />
          )}
        </div>
      </div>

      <div className="mail-attachments">
        <div className="attachment-content">
          <Icon
            icon="material-symbols-light:attachment"
            color="#b31312"
            width="30"
          />
          <span>{attachment.fileName}</span>
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
  ) : (
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
            <button className="mail-action-btns" onClick={handleDelete}>
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
            <Icon icon="bi:ticket-fill" color="#b31312" width="25" />
            <span>{user}</span>
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
        </div>

        <div className="mail-attachments">
          {attachment ? (
            <div className="inbox-mail-attachment">
              <div>
                {attachment.contentType === "text/plain" && (
                  <div>
                    <a
                      href={`data:text/plain;base64,${attachment.data}`}
                      download={attachment.fileName}
                    >
                      {attachment.fileName}
                    </a>
                  </div>
                )}

                {attachment.contentType === "application/pdf" && (
                  <a
                    href={`data:application/pdf;base64,${attachment.data}`}
                    target="_blank"
                  >
                    {attachment.fileName}{" "}
                  </a>
                )}

                {["image/jpeg", "image/png", "image/gif"].includes(
                  attachment.contentType
                ) && (
                  <a
                    href={`data:${attachment.contentType};base64,${attachment.data}`}
                    target="_blank"
                  >
                    {attachment.fileName}{" "}
                  </a>
                )}
              </div>
            </div>
          ) : null}
        </div>
      </div>

      <div className="inbox-mail-summary">
        <p>
          <span>Kime:</span> {mail.recipientsEmail}
        </p>
        <p>
          <span>Konu:</span> {mail.emailSubject}
        </p>
        <p>
          <span>Gönderilme Tarihi:</span> {formattedTime} - {formattedDate}
        </p>
      </div>
    </div>
  )
}

export default Inbox
