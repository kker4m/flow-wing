import React, { useEffect, useState } from "react"
import "./inbox.css"
import { Icon } from "@iconify/react"
import { Tooltip } from "@mui/material"
import { Avatar, Button, Divider, Mentions, Modal } from "antd"
import { useNavigate, useParams } from "react-router"
import Spinner from "../../components/Spinner"
import alertify from "alertifyjs"
import {
  deleteSentEmail,
  forwardEmail,
  getEmailById,
  getForwardedMailById,
  getMailAnswersById,
  replyMail
} from "../../services/emailService"
import { formatDate, getText } from "../../helpers"
import { getUsers } from "../../services/userService"
import ReactQuill from "react-quill"
import { HOME_ROUTE } from "../../routes/index"

const Inbox = () => {
  const [mail, setMail] = useState(null)
  const [attachment, setAttachment] = useState("")
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
  let navigate = useNavigate()
  let { id } = useParams()

  // GET USERS FOR MENTION
  useEffect(() => {
    getUsers()
      .then((res) => {
        setUsers(res.data)
      })
      .catch((error) => {
        console.error("Error fetching users:", error)
      })

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
  // ANTD MENTION FUNCTIONS

  const onMentionSelect = (option) => {
    console.log("select", option)
    setForwardTo(option.value)
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
      console.log("get mail by id", res.data)
      if (res.data.attachments && res.data.attachments.length > 0) {
        setAttachment(res.data.attachments[0])
      }
      console.log("attachment: ", attachment)
      setSender(res.data.sender)
      console.log("mail sender", sender)
      setUser(res.data.emailLog.user.username)
      console.log("user", res.data.emailLog.user.username)
    })

    getMailAnswersById(id).then((response) => {
      console.log("get answerın içindesin şu an")
      console.log(" answer : ", response.data.answer)
      setAnswer(response.data.answer)
      setForwardedMailId(response.data.emailLog.forwardedFrom)
    })

    getForwardedMailById(forwardedMailId).then((res) => {
      console.log("get forwardın içindesin şu an")
      console.log("forwarded mail ", res.data.emailLog.forwardedFrom)
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
      alertify.success("Mail silindi.")
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
      setMail(res.data.emailLog)
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
      }
    })

    navigate()
  }
  return sender === false ? (
    <div className="inbox-page-content">
      <div className="mail-actions">
        <Tooltip title="İlet" arrow onClick={showForwardModal}>
          <div className="icons">
            <button className="mail-action-btns">
              <Icon
                icon="solar:multiple-forward-right-bold-duotone"
                width="35"
                height="35"
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
            <Mentions
              allowClear
              style={{ height: 50, border: "none" }}
              onSelect={onMentionSelect}
              required
              options={options}
              prefix={[""]}
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
              dangerouslySetInnerHTML={{ __html: getText(mail.sentEmailBody) }}
            />
          </form>
        </Modal>
        <Tooltip title="Yanıtla" arrow onClick={showModal}>
          <div className="icons">
            <button className="mail-action-btns">
              <Icon icon="ic:round-reply" width="40" height="40" />
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
              onChange={(event) => {
                setReplyAttachment(event.currentTarget.files[0])
              }}
            />
          </form>
        </Modal>
        <Tooltip title="Sil" arrow>
          <div className="icons">
            <button className="mail-action-btns" onClick={handleDelete}>
              <Icon icon="iconoir:trash-solid" width="35" height="35" />
            </button>
          </div>
        </Tooltip>
      </div>

      <Divider />

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
        <h3>{mail.emailSubject}</h3>
      </div>
      <p dangerouslySetInnerHTML={{ __html: getText(mail.sentEmailBody) }} />

      <div className="mail-attachments">
        <Icon
          icon="material-symbols-light:attachment"
          color="#b31312"
          width="30"
        />
        {/* ATTACHMENT */}
        <div>
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

      {/* ANSWER */}
      <div className="mail-answers">
        {answer ? (
          Array.isArray(answer) ? (
            answer.map((answer) => (
              <div className="mail-answer-content">
                <div className="mail-actions">
                  <Tooltip title="İlet" arrow onClick={showForwardModal}>
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
                      <Button
                        key="submit"
                        type="primary"
                        onClick={handleForwardOk}
                      >
                        İlet
                      </Button>
                    ]}
                  >
                    <form className="forward-modal-form">
                      <label>Kime:</label>{" "}
                      <Mentions
                        allowClear
                        style={{ height: 50, border: "none" }}
                        onSelect={onMentionSelect}
                        required
                        options={options}
                        prefix={[""]}
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
                        <Icon icon="iconoir:reply" width="30" />
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
                        onChange={(event) => {
                          setReplyAttachment(event.currentTarget.files[0])
                        }}
                      />
                    </form>
                  </Modal>
                  <Tooltip title="Sil" arrow>
                    <div className="icons">
                      <button
                        className="mail-action-btns"
                        onClick={handleDelete}
                      >
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
                    <div className="mail-sender-email">
                      {answer?.emailLog.senderEmail}
                    </div>
                  </div>
                </div>

                <div className="mail-title">
                  <Icon icon="uit:subject" color="#b31312" width="40" />{" "}
                  <h3>{answer?.emailLog.emailSubject}</h3>
                </div>

                <p
                  dangerouslySetInnerHTML={{
                    __html: getText(answer?.emailLog.sentEmailBody)
                  }}
                />
                <div className="mail-attachments">
                  <div className="attachment-content">
                    <Icon
                      icon="material-symbols-light:attachment"
                      color="#b31312"
                      width="30"
                    />
                  </div>

                  <div className="mail-attachments">
                    {answer.attachmentInfos ? (

                    
                      <div className="inbox-mail-attachment"> <p>mailin attachmentı var </p> 
                        <div>
                          {answer.attachmentInfos.contentType ===
                            "text/plain" && (
                            <div>
                              <a
                                href={`data:text/plain;base64,${answer.attachment.data}`}
                                download={answer.attachmentInfos.fileName}
                              >
                                {answer.attachmentInfos.fileName}
                              </a>
                            </div>
                          )}

                          {answer.attachmentInfos.contentType ===
                            "application/pdf" && (
                            <a
                              href={`data:application/pdf;base64,${answer.attachmentInfos.data}`}
                              target="_blank"
                            >
                              {answer.attachmentInfos.fileName}{" "}
                            </a>
                          )}

                          {["image/jpeg", "image/png", "image/gif"].includes(
                            answer.attachmentInfos.contentType
                          ) && (
                            <a
                              href={`data:${answer.attachmentInfos.contentType};base64,${attachment.data}`}
                              target="_blank"
                            >
                              {answer.attachmentInfos.fileName}{" "}
                            </a>
                          )}
                        </div>
                      </div>
                    ) : null}
                  </div>
                </div>
              </div>
            ))
          ) : (
            <div className="mail-answer-content">
              <div className="mail-actions">
                <Tooltip title="İlet" arrow onClick={showForwardModal}>
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
                    <Button
                      key="submit"
                      type="primary"
                      onClick={handleForwardOk}
                    >
                      İlet
                    </Button>
                  ]}
                >
                  <form className="forward-modal-form">
                    <label>Kime:</label>{" "}
                    <Mentions
                      allowClear
                      style={{ height: 50, border: "none" }}
                      onSelect={onMentionSelect}
                      required
                      options={options}
                      prefix={[""]}
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
                      <Icon icon="iconoir:reply" width="30" />
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
                      onChange={(event) => {
                        setReplyAttachment(event.currentTarget.files[0])
                      }}
                    />
                  </form>
                </Modal>
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
                  <div className="mail-sender-email">
                    {answer?.emailLog.senderEmail}
                  </div>
                </div>
              </div>

              <div className="mail-title">
                <Icon icon="uit:subject" color="#b31312" width="40" />{" "}
                <h3>{answer?.emailLog.emailSubject}</h3>
              </div>

              <p
                dangerouslySetInnerHTML={{
                  __html: getText(answer?.emailLog.sentEmailBody)
                }}
              />
              <div className="mail-attachments">
                <div className="attachment-content">
                  <Icon
                    icon="material-symbols-light:attachment"
                    color="#b31312"
                    width="30"
                  />
                </div>

                <div className="mail-attachments">
                  {answer.attachmentInfos ? (
                    <div className="inbox-mail-attachment">
                      <div>
                        {answer.attachmentInfos.contentType ===
                          "text/plain" && (
                          <div>
                            <a
                              href={`data:text/plain;base64,${answer.attachment.data}`}
                              download={answer.attachmentInfos.fileName}
                            >
                              {answer.attachmentInfos.fileName}
                            </a>
                          </div>
                        )}

                        {answer.attachmentInfos.contentType ===
                          "application/pdf" && (
                          <a
                            href={`data:application/pdf;base64,${answer.attachmentInfos.data}`}
                            target="_blank"
                          >
                            {answer.attachmentInfos.fileName}{" "}
                          </a>
                        )}

                        {["image/jpeg", "image/png", "image/gif"].includes(
                          answer.attachmentInfos.contentType
                        ) && (
                          <a
                            href={`data:${answer.attachmentInfos.contentType};base64,${attachment.data}`}
                            target="_blank"
                          >
                            {answer.attachmentInfos.fileName}{" "}
                          </a>
                        )}
                      </div>
                    </div>
                  ) : null}
                </div>
              </div>
            </div>
          )
        ) : null}
      </div>

      {/* FORWARDED */}

      {forwardedFrom ? (
        <div className="forwarded-from-section">
          <span>----- Şu mesajdan iletildi -----</span>
          <span>Gönderen: </span> <p>{forwardedFrom.senderEmail}</p>
          <span>Tarih:</span> <p> {formatDate(forwardedFrom.sentDateTime)}</p>
          <span>Konu:</span> <p> {forwardedFrom.emailSubject}</p>
          <p
            dangerouslySetInnerHTML={{
              __html: getText(forwardedFrom.sentEmailBody)
            }}
          />
        </div>
      ) : null}
    </div>
  ) : null
}

export default Inbox
