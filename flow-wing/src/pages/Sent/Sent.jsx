import React, { useEffect, useState } from "react"
import { Icon } from "@iconify/react"
import { Link, useNavigate, useParams } from "react-router-dom"
import Divider from "@mui/material/Divider"
import "./sent.css"
import { excerpt, getText } from "../../helpers"
import { deleteSentEmail, getSentMails } from "../../services/emailService"
import EmptyPage from "../../components/EmptyPage"

const Sent = () => {
  const [sentMails, setSentMails] = useState([])
  const [mailCount, setMailCount] = useState(0)
  let navigate = useNavigate()
  let { id } = useParams()

  // get sent emails
  useEffect(() => {
    getSentMails().then((response) => {
      // Sort the sentMails array by sentDateTime in descending order
      const sortedMails = response.data.userEmails.sort(
        (a, b) =>
          new Date(b.emailLog.sentDateTime) - new Date(a.emailLog.sentDateTime)
      )
      setMailCount(response.data.userEmails.length)
      setSentMails(sortedMails)
    })
  }, [])
  // SPINNER
  if (sentMails.length === 0) {
    return <EmptyPage />
  }
  // COLOR ARRAY FOR HR ELEMENT
  const colors = ["#C0440E", "#3498db", "#27ae60", "#f39c12", "#8e44ad"]

  // delete email
  const handleDelete = (id) => {
    deleteSentEmail(id).then((res) => {
      console.log(res)
      // Update the sentMails state after deleting the email
      setSentMails(sentMails.filter((mail) => mail.id !== id))
      navigate("/sent")
    })
  }

  return (
    <div className="sent-mail-page-content">
      <h2>{mailCount} mesaj</h2>
      <div className="sent">
        {sentMails.map((item, index) => {
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
              <Link to={`/sentbox/${item.emailLog.id}`}>
                <div className="sent-mail-content" key={index}>
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
                        {item.emailLog.recipientsEmail}{" "}
                      </div>
                    </div>
                    <div className="inbox-mail-title">
                      {item.emailLog.emailSubject}
                    </div>
                    <div className="inbox-mail-body">
                      <p
                        dangerouslySetInnerHTML={{
                          __html: getText(
                            excerpt(item.emailLog.sentEmailBody, 120)
                          )
                        }}
                      />
                    </div>
                  </div>{" "}
                  <div className="repeat-delete-sent-time-section">
                    <div className="is-repeating-icon">
                      {item.emailLog.isScheduled === true && (
                        <Icon icon="bi:repeat" />
                      )}
                    </div>{" "}
                    {/* <div className="delete-mail">
                      {" "}
                      <button
                        className="delete-mail-btn"
                        onClick={() => handleDelete(item.emailLog.id)}
                      >
                        <Icon icon="iconoir:trash" />
                      </button>
                    </div>{" "} */}
                    <div className="inbox-sent-time">{timeToShow}</div>
                  </div>
                </div>{" "}
                <Divider />{" "}
              </Link>
            </>
          )
        })}
      </div>
    </div>
  )
}

export default Sent
