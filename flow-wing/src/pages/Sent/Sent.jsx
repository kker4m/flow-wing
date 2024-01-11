import React, { useEffect, useState } from 'react'
import data from "../../data.json";
import { Icon } from "@iconify/react";
import { Link } from "react-router-dom";
import Divider from "@mui/material/Divider";
import EmailService from '../../services/emailService';
import "./sent.css"

const Sent = () => {
const [sentMails,setSentMails]= useState()

   // to shorten the mail body
   const excerpt = (str, count) => {
    if (str && str.length > count) {
      str = str.substring(0, count) + "...";
    }
    return str;
  };

const emailService = new EmailService()

  useEffect(()=>{
    emailService.getSentMails().then(res=>setSentMails(res.data))
  },[])

  // COLOR ARRAY FOR HR ELEMENT
  const colors = ["#C0440E", "#3498db", "#27ae60", "#f39c12", "#8e44ad"]; // İstediğiniz renkleri ekleyin
  return (
    <div className='sent-mail-page-content'>
      <h2>Gönderilmiş Mailler</h2>
      <div className="sent">
        {data.map((item, index) => (
          <Link to={`/inbox/${index}`} key={index}>
          
            <div className="mail-content">
            <hr style={{ border: `1px solid ${colors[index % colors.length]}` }} />
              <div key={index} className="inbox-mail-unopened">
                <div className="user-section">
                  <div className="user-icon-home">
                    <Icon icon="ph:user-light" width="30" />{" "}
                  </div>

                  <div className="user-name">{item.sender} </div>
                </div>
                <div className="inbox-mail-title">{item.title}</div>
                <div className="inbox-mail-body">{excerpt(item.body, 120)}</div>
              </div>{" "}
              <div className="inbox-sent-time">{item.sentTime}</div>
            </div>{" "}
            <Divider />
          </Link>
        ))}
      </div>
      
      </div>
  )
}

export default Sent