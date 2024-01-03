import React, { useState } from "react";
import Attachments from "../../components/Attachments";
import { Input } from "antd";
import TextArea from "antd/es/input/TextArea";

import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css'; // Stil dosyasını ekleyin
import "./compose.css";
import { useSelector } from "react-redux";
import { Divider } from "@mui/material";
import EmailService from "../../services/emailService";
import { useFormik } from "formik";

const Compose = () => {

// GET USER 
  const user = useSelector((state) => state.user.user);

  // MAIL SEND FUNCTION
  const handleSubmit = (values)=>{
    let emailService = new EmailService();
    emailService.sendMail(values)
  }
  // FORMIK

const formik = useFormik({
  initialValues:{
    recipientsEmail:"",
    emailSubject:"",
    emailBody:""
  },
  onSubmit: (values)=>{
    handleSubmit(values)
  }
})


  
 
  return (
    <div className="compose-page-content">
      
      <div className="compose send-to">
       <span>Kime</span> <input
       type="text"
       name="recipientsEmail"
       onChange={formik.handleChange}
       value={formik.values.recipientsEmail}
       ></input>
        <Divider />
      </div>
      <div className="compose mail-title">
      <span>Konu</span>  <input      type="text"
       name="emailSubject"
       onChange={formik.handleChange}
       value={formik.values.emailSubject}></input>
        <Divider />
      </div>
      <div className="compose mail-body">
    <textarea
      name="emailBody"
      value={formik.values.emailBody}
      onChange={formik.handleChange}
type="text"
    />
  </div>
      <div className="compose-attachments">
      <Attachments/>
         <Divider type="vertical" />
      </div>
      <div className="compose-btns">
        <button className="send-btn" type="submit" onClick={formik.handleSubmit}>Gönder</button>
        <button className="delete-btn">Sil</button>
      </div>
    </div>
  );
};

export default Compose;
