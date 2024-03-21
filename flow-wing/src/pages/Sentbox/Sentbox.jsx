import React, { useEffect, useState } from "react"
import "./sentbox.css"
import { useNavigate, useParams } from "react-router"
import {
  deleteSentEmail,
  forwardEmail,
  getEmailAndAnswersByEmailLogId,
  getEmailById,
  getForwardedMailById,
  getMailAnswersById,
  replyMail
} from "../../services/emailService"
import Spinner from "../../components/Spinner"
import { Avatar, Button, Divider, Modal, Tooltip } from "antd"
import { Icon } from "@iconify/react"
import ReactQuill from "react-quill"
import { formatDate, getText } from "../../helpers"
import alertify from "alertifyjs"
import { HOME_ROUTE } from "../../routes"

const Sentbox = () => {
  const [mail, setMail] = useState(null)
  const [attachments, setAttachments] = useState([])
  const [sender, setSender] = useState(true)
  const [user, setUser] = useState("")
  const [answer, setAnswer] = useState("")
  const [loading, setLoading] = useState(false)
  const [open, setOpen] = useState(false)
  const [forwardMOdalOpen, setForwardModalOpen] = useState(false)
  const [repliedMailBody, setRepliedMailBody] = useState("")
  const [forwardTo, setForwardTo] = useState("")
  const [forwardedEmailMessage, setForwardedEmailMessage] = useState("")
  const [forwardedMailId, setForwardedMailId] = useState("")
  const [forwardedFrom, setForwardedFrom] = useState("")
  const [users, setUsers] = useState([])
  const [replyAttachment, setReplyAttachment] = useState("")
  const [answerArray, setAnswerArray] = useState([])
  let navigate = useNavigate()
  let { id } = useParams()

  // GET ANSWER INFOS

  useEffect(() => {
    return () => {}
  }, [])
  const options = users.map((user) => ({
    value: user.email,
    label: user.email
  }))
  // MODAL
  const showModal = () => {
    setOpen(true)
  }

  const showForwardModal = () => {
    setForwardModalOpen(true)
  }
  const handleOk = () => {
    if (!repliedMailBody) {
      alertify.error("Lütfen geçerli bir mesaj girin")
    } else {
      replyEmail()
      setOpen(false)
    }
  }
  const handleForwardOk = () => {
    if (!forwardTo || !forwardedEmailMessage) {
      alertify.error("Lütfen tüm alanları doldurun")
    } else {
      setForwardModalOpen(false)
      handleForward()
    }
  }
  const handleCancel = () => {
    setOpen(false)
    setForwardModalOpen(false)
  }

  // QUILL TOOLBAR
  const toolbarOptions = {
    toolbar: [
      [{ font: [] }],
      [{ header: [1, 2, 3] }],
      ["bold", "italic", "underline", "strike"],
      [{ color: [] }, { background: [] }],
      [{ script: "sub" }, { script: "super" }],
      ["blockquote", "code-block"],
      [{ list: "ordered" }, { list: "bullet" }],
      [{ indent: "-1" }, { indent: "+1" }, { align: [] }],
      ["link", "image", "video"],
      ["clean"]
    ]
  }

  // GET SINGLE MAIL BY ID

  useEffect(() => {
    getEmailById(id).then((res) => {
      setMail(res.data.emailLog)
      //console.log("get mail by id", res.data)

      setAttachments(res.data.attachments)
      console.log("attachment: ", res.data.attachments)
      console.log("attachment2: ", attachments)
      setSender(res.data.sender)
      //console.log("mail sender", sender)
      setUser(res.data.emailLog.user.username)
      //console.log("user", res.data.emailLog.user.username)
    })

    getEmailAndAnswersByEmailLogId(id).then((response) => {
      console.log("info endpoint :", response)
      console.log("info endpoint answers :", response.data.answers)
      setAnswerArray(response.data.answers)
      console.log("forwarded mail ", response.data.forwardedEmailLog)
      setForwardedFrom(response.data.forwardedEmailLog)
      console.log(" answer : ", response.data.answer)
      setAnswer(response.data.answer)
      console.log("answer emaillog ", answer?.emailLog?.sentEmailBody)
      setForwardedMailId(response.data.emailLog.forwardedFrom)
      console.log("forwarded mail id", forwardedMailId)
    })

    getForwardedMailById(forwardedMailId).then((res) => {
      console.log("forwarded mail ", res.data.forwardedEmailLog)
      setForwardedFrom(res.data.emailLog)
      setForwardedMailId(res.data.emailLog.forwardedFrom)
    })

    return () => {}
  }, [mail?.forwardedFrom])

  // SPINNER
  if (!mail) {
    return <Spinner />
  }
  // DELETE AN EMAIL
  const handleDelete = () => {
    deleteSentEmail(mail.id).then((res) => {
      console.log(res)
      if (res.status === 200) {
        alertify.success("Mail silindi")
      } else alertify.error(res.message)
      navigate(HOME_ROUTE)
    })
  }

  // REPLY AN EMAIL

  const replyEmail = () => {
    const values = {
      recipientsEmail: mail.recipientsEmail,
      emailSubject: mail.emailSubject,
      emailBody: repliedMailBody,
      RepliedEmailId: mail.answer ? mail.answer : mail.id,
      file: replyAttachment
    }
    const formData = new FormData()
    if (values.file && values.file.length > 0) {
      formData.append("attachment", values.file[0])
    } else {
      formData == []
    }
    replyMail(values, formData).then((res) => {
      if (res.status === 400) {
        alertify.error("Bu mail yalnızca bir kez yanıtlanabilir")
      } else if (res.status === 201) {
        setMail(res.data.emailLog)
        alertify.success("Mail yanıtlandı")
      }
    })

    navigate()
  }

  // FORWARD EMAIL

  const handleForward = () => {
    const values = {
      recipientsEmail: forwardTo,
      emailSubject: mail.emailSubject,
      emailBody: forwardedEmailMessage,
      ForwardedEmailId: mail.id,
      file: []
    }
    console.log("handleForward'ın içindesin")
    forwardEmail(values).then((res) => {
      console.log("forward mail fonksiyonunun içindesin")
      if (res.status === 201) {
        alertify.success("Mail iletildi")
      } else alertify.error(res.message)
    })

    navigate()
  }

  return (
    <div className="inbox-page-content">
      {/* MAIL ACTIONS  */}

      <div className="mail-actions">
        <Tooltip title="İlet" arrow onClick={showForwardModal}>
          <div className="icons">
            <button className="mail-action-btns">
              <Icon
                icon="solar:multiple-forward-right-bold-duotone"
                width="25"
                height="25"
              />
            </button>
          </div>
        </Tooltip>

        {/* MODAL FOR FORWARD EMAIL */}
        <Modal
          open={forwardMOdalOpen}
          title="İLET"
          onOk={handleForwardOk}
          onCancel={handleCancel}
          footer={[
            <Button key="back" onClick={handleCancel}>
              Geri
            </Button>,
            <Button key="submit" type="primary" onClick={handleForwardOk}>
              Gönder
            </Button>
          ]}
        >
          <form className="forward-modal-form">
            <label>Kime: </label>{" "}
            <input
              style={{ height: 50, border: "none" }}
              required
              onChange={(e) => {
                setForwardTo(e.target.value)
              }}
            />
            <label>Mesaj: </label>{" "}
            <ReactQuill
              modules={toolbarOptions}
              theme="bubble"
              name="emailBody"
              style={{ height: 150, boxShadow: "rgba(0, 0, 0, 0.1)" }}
              onChange={(value) => {
                setForwardedEmailMessage(value)
              }}
              required
            />
            <br />
            <br />
            <br />
            <br />
            <span>-------Şu mesaj iletilecek-------</span>
            <p>
              {" "}
              <span>Gönderen:</span> {mail.senderEmail}
            </p>
            <p>
              {" "}
              <span>Tarih: </span> {formatDate(mail.sentDateTime)}{" "}
            </p>
            <p>
              {" "}
              <span>Konu:</span> {mail.emailSubject}
            </p>
            <p
              dangerouslySetInnerHTML={{ __html: getText(mail.sentEmailBody) }}
            />
          </form>
        </Modal>

        <Tooltip title="Yanıtla" arrow onClick={showModal}>
          <div className="icons">
            <button className="mail-action-btns">
              <Icon icon="ic:round-reply" width="30" height="30" />
            </button>
          </div>{" "}
        </Tooltip>
        {/* MODAL FOR REPLY EMAIL */}
        <Modal
          open={open}
          title="YANITLA"
          onOk={handleOk}
          onCancel={handleCancel}
          footer={[
            <Button key="back" onClick={handleCancel}>
              Geri
            </Button>,
            <Button
              key="submit"
              type="primary"
              loading={loading}
              onClick={handleOk}
            >
              Gönder
            </Button>
          ]}
        >
          <form className="reply-modal-form">
            <label type="text">
              <span>Kime: </span>
              {mail.recipientsEmail}
            </label>
            <label type="text">
              <span>Konu: </span>
              {mail.emailSubject}
            </label>
            <span>Mesaj: </span>
            <ReactQuill
              modules={toolbarOptions}
              theme="bubble"
              name="emailBody"
              style={{ height: 150, boxShadow: "rgba(0, 0, 0, 0.1)" }}
              onChange={(value) => setRepliedMailBody(value)}
              required
            />

            <input
              id="attachment"
              name="attachment"
              type="file"
              multiple
              onChange={(event) => {
                const selectedFiles = event.currentTarget.files
                // Tüm dosyaları içeren bir nesne oluşturun
                const filesObject = {}
                for (let i = 0; i < selectedFiles.length; i++) {
                  const file = selectedFiles[i]
                  filesObject[`file[${i}]`] = file
                }
                // Dosyaları bir nesne olarak ayarlayın
                setReplyAttachment(filesObject)
              }}
            />
          </form>
        </Modal>
        <Tooltip title="Sil" arrow>
          <div className="icons">
            <button className="mail-action-btns" onClick={handleDelete}>
              <Icon icon="iconoir:trash-solid" width="25" height="25" />
            </button>
          </div>
        </Tooltip>
      </div>
      <Divider />
      {/* MAIL SECTION */}
      <div className="mail-sender">
        <div className="user-icon-inbox">
          <Avatar
            style={{
              backgroundColor: "#fde3cf",
              color: "#f56a00",
              width: 60,
              height: 60
            }}
          >
            <span>{user.charAt(0)}</span>
          </Avatar>
        </div>
        <div>
          <div className="mail-sender-email">{mail.senderEmail}</div>
        </div>
      </div>

      <div className="mail-title">
        <p>{mail.emailSubject}</p>
      </div>
      <p dangerouslySetInnerHTML={{ __html: getText(mail.sentEmailBody) }} />
      {/* MAIL ATTACHMENT SECTION */}
      <div className="mail-attachments">
        {/* ATTACHMENT */}
        <div className="inbox-mail-attachment-content">
          {attachments?.map((attachments) => (
            <div className="inbox-mail-attachment">
              <div>
                {" "}
                <Icon icon="et:attachments" width="16" height="16" />
                {attachments.contentType === "text/plain" && (
                  <div>
                    <a
                      href={`data:text/plain;base64,${attachments.data}`}
                      download={attachments.fileName}
                    >
                      {attachments.fileName}
                    </a>
                  </div>
                )}
                {attachments.contentType ===
                  "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && (
                  <a
                    href={`data:application/vnd.openxmlformats-officedocument.wordprocessingml.document;base64,${attachments.data}`}
                    target="_blank"
                    download={attachments.fileName}
                  >
                    {attachments.fileName}
                  </a>
                )}
                {attachments.contentType === "application/pdf" && (
                  <a
                    href={`data:application/pdf;base64,${attachments.data}`}
                    target="_blank"
                  >
                    {attachments.fileName}{" "}
                  </a>
                )}
                {["image/jpeg", "image/png", "image/gif"].includes(
                  attachments.contentType
                ) && (
                  <a
                    href={`data:${attachments.contentType};base64,${attachments.data}`}
                    target="_blank"
                  >
                    {attachments.fileName}{" "}
                  </a>
                )}
                {["application/octet-stream", "application/zip"].includes(
                  attachments.contentType
                ) && (
                  <div>
                    <a
                      href={`data:application/octet-stream;base64,${attachments.data}`}
                      download={attachments.fileName}
                    >
                      {attachments.fileName}
                    </a>
                  </div>
                )} {/* İndirme Düğmesi */}
      <button className="attachment-download-btn"
        onClick={() => {
          const link = document.createElement("a");
          link.href = `data:${attachments.contentType};base64,${attachments.data}`;
          link.download = attachments.fileName;
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        }}
      >
        İndir
      </button>
              </div>
              {}{" "}
            </div>
          ))}
        </div>
      </div>

      <Divider />

      {/* MAIL ANSWER SECTION */}
      {answerArray.map((answer) => (
        <div className="mail-answers">
          <div className="mail-answer-content">
            {/* MAIL ANSWER ACTIONS */}
            <div className="mail-actions">
              <Tooltip title="İlet" arrow onClick={showForwardModal}>
                <div className="icons">
                  <button className="mail-action-btns">
                    <Icon
                      icon="solar:multiple-forward-right-bold-duotone"
                      width="25"
                      height="25"
                    />
                  </button>
                </div>
              </Tooltip>

              {/* MODAL FOR FORWARD EMAIL */}
              <Modal
                open={forwardMOdalOpen}
                title="İLET"
                onOk={handleForwardOk}
                onCancel={handleCancel}
                footer={[
                  <Button key="back" onClick={handleCancel}>
                    Geri
                  </Button>,
                  <Button key="submit" type="primary" onClick={handleForwardOk}>
                    Gönder
                  </Button>
                ]}
              >
                <form className="forward-modal-form">
                  <label>Kime: </label>{" "}
                  <input
                    style={{ height: 50, border: "none" }}
                    required
                    onChange={(e) => {
                      setForwardTo(e.target.value)
                    }}
                  />
                  <label>Mesaj: </label>{" "}
                  <ReactQuill
                    modules={toolbarOptions}
                    theme="bubble"
                    name="emailBody"
                    style={{ height: 150, boxShadow: "rgba(0, 0, 0, 0.1)" }}
                    onChange={(value) => {
                      setForwardedEmailMessage(value)
                    }}
                    required
                  />
                  <br />
                  <br />
                  <br />
                  <br />
                  <span>-------Şu mesajdan iletilecek-------</span>
                  <p>
                    {" "}
                    <span>Gönderen:</span> {mail.senderEmail}
                  </p>
                  <p>
                    {" "}
                    <span>Tarih: </span> {formatDate(mail.sentDateTime)}{" "}
                  </p>
                  <p>
                    {" "}
                    <span>Konu:</span> {mail.emailSubject}
                  </p>
                  <p
                    dangerouslySetInnerHTML={{
                      __html: getText(mail.sentEmailBody)
                    }}
                  />
                </form>
              </Modal>

              <Tooltip title="Yanıtla" arrow onClick={showModal}>
                <div className="icons">
                  <button className="mail-action-btns">
                    <Icon icon="ic:round-reply" width="30" height="30" />
                  </button>
                </div>{" "}
              </Tooltip>
              {/* MODAL FOR REPLY EMAIL */}
              <Modal
                open={open}
                title="YANITLA"
                onOk={handleOk}
                onCancel={handleCancel}
                footer={[
                  <Button key="back" onClick={handleCancel}>
                    Geri
                  </Button>,
                  <Button
                    key="submit"
                    type="primary"
                    loading={loading}
                    onClick={handleOk}
                  >
                    Gönder
                  </Button>
                ]}
              >
                <form className="reply-modal-form">
                  <label type="text">
                    <span>Kime: </span>
                    {mail.recipientsEmail}
                  </label>
                  <label type="text">
                    <span>Konu: </span>
                    {mail.emailSubject}
                  </label>
                  <span>Mesaj: </span>
                  <ReactQuill
                    modules={toolbarOptions}
                    theme="bubble"
                    name="emailBody"
                    style={{ height: 150, boxShadow: "rgba(0, 0, 0, 0.1)" }}
                    onChange={(value) => setRepliedMailBody(value)}
                    required
                  />
                  <input
                    id="attachment"
                    name="attachment"
                    type="file"
                    multiple
                    onChange={(event) => {
                      const selectedFiles = event.currentTarget.files
                      // Tüm dosyaları içeren bir nesne oluşturun
                      const filesObject = {}
                      for (let i = 0; i < selectedFiles.length; i++) {
                        const file = selectedFiles[i]
                        filesObject[`file[${i}]`] = file
                      }
                      // Dosyaları bir nesne olarak ayarlayın
                      setReplyAttachment(filesObject)
                    }}
                  />
                </form>
              </Modal>
              <Tooltip title="Sil" arrow>
                <div className="icons">
                  <button className="mail-action-btns" onClick={handleDelete}>
                    <Icon icon="iconoir:trash-solid" width="25" height="25" />
                  </button>
                </div>
              </Tooltip>
            </div>

            <div className="mail-sender">
              <div className="user-icon-inbox">
                <Avatar
                  style={{
                    backgroundColor: "#fde3cf",
                    color: "#f56a00",
                    width: 60,
                    height: 60
                  }}
                >
                  <span>{user.charAt(0)}</span>
                </Avatar>
              </div>
              <div>
                <div className="mail-sender-email">
                  {answer?.emailLog.senderEmail}
                </div>
              </div>
            </div>

            <div className="mail-title">
              <p>{answer?.emailLog.emailSubject}</p>
            </div>

            <p
              dangerouslySetInnerHTML={{
                __html: getText(answer?.emailLog.sentEmailBody)
              }}
            />
          </div>{" "}
          {/* MAIL ANSWERS ATTACHMENTS */}
          {answerArray?.attachmentInfos?.map((attachments) => (
            <div className="inbox-mail-attachment">
              <div>
                {" "}
                <Icon icon="et:attachments" width="16" height="16" />
                {attachments.contentType === "text/plain" && (
                  <div>
                    <a
                      href={`data:text/plain;base64,${attachments.data}`}
                      download={attachments.fileName}
                    >
                      {attachments.fileName}
                    </a>
                  </div>
                )}
                {attachments.contentType ===
                  "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && (
                  <a
                    href={`data:application/vnd.openxmlformats-officedocument.wordprocessingml.document;base64,${attachments.data}`}
                    target="_blank"
                    download={attachments.fileName}
                  >
                    {attachments.fileName}
                  </a>
                )}
                {attachments.contentType === "application/pdf" && (
                  <a
                    href={`data:application/pdf;base64,${attachments.data}`}
                    target="_blank"
                  >
                    {attachments.fileName}{" "}
                  </a>
                )}
                {["image/jpeg", "image/png", "image/gif"].includes(
                  attachments.contentType
                ) && (
                  <a
                    href={`data:${attachments.contentType};base64,${attachments.data}`}
                    target="_blank"
                  >
                    {attachments.fileName}{" "}
                  </a>
                )}
                {["application/octet-stream", "application/zip"].includes(
                  attachments.contentType
                ) && (
                  <div>
                    <a
                      href={`data:application/octet-stream;base64,${attachments.data}`}
                      download={attachments.fileName}
                    >
                      {attachments.fileName}
                    </a>
                  </div>
                )} {/* İndirme Düğmesi */}
      <button
        onClick={() => {
          const link = document.createElement("a");
          link.href = `data:${attachments.contentType};base64,${attachments.data}`;
          link.download = attachments.fileName;
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        }}
      >
        İndir
      </button>
              </div>
              {}{" "}
            </div>
          ))}

        </div>
        
      ))}

      {/* IS FORWARDED SECTION */}

      {forwardedFrom ? (
        <div className="forwarded-from-section">
          <span>----- Şu mesaj iletildi -----</span>
          <span>Gönderen: </span> <p>{forwardedFrom.senderEmail}</p>
          <span>Tarih:</span> <p> {formatDate(forwardedFrom.sentDateTime)}</p>
          <span>Konu:</span> <p> {forwardedFrom.emailSubject}</p>
          <p
            dangerouslySetInnerHTML={{
              __html: getText(forwardedFrom.sentEmailBody)
            }}
          />
          {/* FORWARDED FROMS ATTACHMENT SECTION */}
          {forwardedFrom.attachmentInfos?.map((attachments) => (
            <div className="inbox-mail-attachment">
              <div>
                {" "}
                <Icon icon="et:attachments" width="16" height="16" />
                {attachments.contentType === "text/plain" && (
                  <div>
                    <a
                      href={`data:text/plain;base64,${attachments.data}`}
                      download={attachments.fileName}
                    >
                      {attachments.fileName}
                    </a>
                  </div>
                )}
                {attachments.contentType ===
                  "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && (
                  <a
                    href={`data:application/vnd.openxmlformats-officedocument.wordprocessingml.document;base64,${attachments.data}`}
                    target="_blank"
                    download={attachments.fileName}
                  >
                    {attachments.fileName}
                  </a>
                )}
                {attachments.contentType === "application/pdf" && (
                  <a
                    href={`data:application/pdf;base64,${attachments.data}`}
                    target="_blank"
                  >
                    {attachments.fileName}{" "}
                  </a>
                )}
                {["image/jpeg", "image/png", "image/gif"].includes(
                  attachments.contentType
                ) && (
                  <a
                    href={`data:${attachments.contentType};base64,${attachments.data}`}
                    target="_blank"
                  >
                    {attachments.fileName}{" "}
                  </a>
                )}
                {["application/octet-stream", "application/zip"].includes(
                  attachments.contentType
                ) && (
                  <div>
                    <a
                      href={`data:application/octet-stream;base64,${attachments.data}`}
                      download={attachments.fileName}
                    >
                      {attachments.fileName}
                    </a>
                  </div>
                )}
              </div>
              {}{" "}
            </div>
          ))}
        </div>
      ) : null}
    </div>
  )
}

export default Sentbox
