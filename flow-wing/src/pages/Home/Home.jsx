import React, { useEffect, useState } from "react"
import "./home.css"
import { Icon } from "@iconify/react"
import { Link, useNavigate } from "react-router-dom"
import Divider from "@mui/material/Divider"
import alertify from "alertifyjs"
import EmptyPage from "../../components/EmptyPage"
import { deleteSentEmail, getMails } from "../../services/emailService"
import { excerpt } from "../../helpers"

const Home = () => {
  const [mails, setMails] = useState([])
  let navigate = useNavigate()

  // Get all e-mails
  useEffect(() => {
    try {
      getMails().then((response) => {
        console.log("response gelen mail: ", response)
        console.log("gelen mailler: ", response.data.userEmails)

        const sortedMails = response.data.userEmails.sort(
          (a, b) => new Date(a.sentDateTime) - new Date(b.sentDateTime)
        )
        console.log("sorted: ", sortedMails)
        // console.log("attachments: ", sortedMails[3].attachments[0].fileName)
        setMails(sortedMails)
      })
    } catch (error) {
      console.error("Error fetching mails:", error)
    }
  }, [])

  // DELETE AN EMAIL
  const handleDelete = (id) => {
    try {
      deleteSentEmail(id).then((res) => {
        console.log(res)
        // Update the sentMails state after deleting the email
        setMails(mails.filter((mail) => mail.id !== id))
        alertify.success("Mail silindi.")
        navigate("/home")
      })
    } catch (error) {
      console.log(error)
    }
  }
  // COLOR ARRAY FOR HR ELEMENT
  const colors = ["#C0440E", "#3498db", "#27ae60", "#f39c12", "#8e44ad"] // İstediğiniz renkleri ekleyin

  if (mails.length === 0) {
    return <EmptyPage />
  }
  return (
    <div className="home-page-content">
      <h2>Gelen Mailler</h2>
      <div className="inbox">
        {mails.map((item, index) => {
          // to format date
          const dateFromAPI = new Date(item.emailLog.sentDateTime)
          const nowsDate = new Date()

          let timeToShow

          if (
            dateFromAPI.getFullYear() === nowsDate.getFullYear() &&
            dateFromAPI.getMonth() === nowsDate.getMonth() &&
            dateFromAPI.getDate() === nowsDate.getDate()
          ) {
            const hourPart = dateFromAPI.toLocaleTimeString("tr-TR", {
              hour: "numeric",
              minute: "numeric"
            })
            timeToShow = hourPart
          } else {
            const datePart = dateFromAPI.toLocaleDateString("tr-TR")
            timeToShow = datePart
          }

          return (
            <>
              <Link to={`/inbox/${item.emailLog.id}`} key={index}>
                <div className="mail-content">
                  <hr
                    style={{
                      border: `1px solid ${colors[index % colors.length]}`
                    }}
                  />
                  <div key={index} className="inbox-mail-unopened">
                    <div className="user-section">
                      <div className="user-icon-home">
                        <Icon icon="ph:user-light" width="30" />{" "}
                      </div>

                      <div className="user-name">
                        {item.emailLog.senderEmail}{" "}
                      </div>
                    </div>
                    <div className="inbox-mail-title">
                      {item.emailLog.emailSubject}
                    </div>
                    <div className="inbox-mail-body">
                      {excerpt(item.emailLog.sentEmailBody, 120)}
                    </div>
                    <div className="inbox-mail-attachment">
  <Icon icon="ri:attachment-fill" width="20" height="20" />
  {item.attachments[0].fileName}
</div>
<div>
  {item.attachments[0].contentType === 'text/plain' && (
    <iframe
      title="Attachment"
      width="600"
      height="400"
      srcDoc={atob(item.attachments[0].data)}
    />
  )}
  {item.attachments[0].contentType === 'application/pdf' && (
    <embed
      src={`data:application/pdf;base64,${item.attachments[0].data}`}
      type="application/pdf"
      width="100"
      height="100"
    />
  )}
  {['image/jpeg', 'image/png', 'image/gif'].includes(item.attachments[0].contentType) && (
    <img
      src={`data:${item.attachments[0].contentType};base64,${item.attachments[0].data}`}
      alt="Attachment"
      width="100"
      height="100"
    />
  )}
</div>
<div className="repeat-delete-sent-time-section">
  <div className="is-repeating-icon">
    {item.emailLog.isScheduled === true && (
      <Icon icon="bi:repeat" />
    )}
  </div>
</div>



                    <div className="delete-mail">
                      {" "}
                      <button
                        className="delete-mail-btn"
                        onClick={() => handleDelete(item.emailLog.id)}
                      >
                        <Icon icon="iconoir:trash" />
                      </button>
                    </div>{" "}
                    <div className="inbox-sent-time">{timeToShow}</div>
                  </div>
                </div>
                <Divider />
              </Link>
            </>
          )
        })}
      </div>
    </div>
  )
}

export default Home
