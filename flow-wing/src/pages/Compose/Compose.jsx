import React, { useState } from "react";
import Attachments from "../../components/Attachments";
import { Divider } from "@mui/material";
import { useSelector } from "react-redux";
import { useFormik } from "formik";
import "./compose.css";
import EmailService from "../../services/emailService";
import { useNavigate } from "react-router";
import alertify from "alertifyjs";
import { Mentions } from "antd";
import * as Yup from "yup";


const Compose = () => {
  const navigate = useNavigate();
  // GET USER
  const user = useSelector((state) => state.user.user);
  let emailService = new EmailService();
  // MAIL SEND FUNCTION
  const handleSubmit = (values) => {
    emailService.sendMail(values).then((res) => {
      if (res.status === 201) {
        alertify.success("Mail Gönderildi");
        navigate("/home");
      } else alertify.error("Gönderme başarısız oldu");
    });
  };
  // ANTD MENTION FUNCTIONS
  const onChange = (value) => {
    console.log("Change:", value);
    formik.handleChange({ target: { name: "recipientsEmail", value } });
  };

  const onSelect = (option) => {
    console.log("select", option);
    formik.setFieldValue("recipientsEmail", option.value);
  };
  //Yup validation schema
  const validationSchema = Yup.object().shape({
    recipientsEmail: Yup.string().email("Geçersiz mail adresi").required("Mail adresi girmek zorunludur"),
    emailSubject: Yup.string().required("Konu giriniz"),
    emailBody: Yup.string().required("İçerik giriniz"),
  });
  // FORMIK

  const formik = useFormik({
    initialValues: {
      recipientsEmail: "",
      emailSubject: "",
      emailBody: "",
    },
    validationSchema: validationSchema,  
    onSubmit: (values) => {
      handleSubmit(values);
    },
  });
// reset and return to home page

const handleReset=()=>{
  
}
  return (
    <div className="compose-page-content">
      <h2>Mail Oluştur</h2>

      <div className="compose send-to">
        <span>Kime</span> <Divider />
        <Mentions
          style={{ height: 50, border: "none" }}
          onChange={onChange}
          onSelect={onSelect}
          value={formik.values.recipientsEmail}
          required
          options={[
            {
              value: "afc163",
              label: "afc163",
            },
            {
              value: "zombieJ",
              label: "zombieJ",
            },
            {
              value: "yesmeck",
              label: "yesmeck",
            },
          ]}
        />
      </div>
      {formik.errors.recipientsEmail && formik.touched.recipientsEmail && (
          <div className="error-message">{formik.errors.recipientsEmail}</div>
        )}
      <div className="compose send-to">
        <span>Konu</span>
        <Divider />
        <input
          type="text"
          name="emailSubject"
          onChange={formik.handleChange}
          value={formik.values.emailSubject}
        ></input>
      </div>
      {formik.errors.emailSubject && formik.touched.emailSubject && (
          <div className="error-message">{formik.errors.emailSubject}</div>
        )}
      <div className="compose mail-body">
        <span>İçerik</span> <Divider />
        <textarea
          name="emailBody"
          value={formik.values.emailBody}
          onChange={formik.handleChange}
        />
      </div>
      {formik.errors.emailBody && formik.touched.emailBody && (
          <div className="error-message">{formik.errors.emailBody}</div>
        )}
      <div className="compose-attachments">
        <Attachments />
        <hr />
      </div>
      <div className="compose-btns">
        <button
          className="send-btn"
          type="submit"
          onClick={formik.handleSubmit}
        >
          Gönder
        </button>
        <button className="delete-btn">Sil</button>
      </div>
    </div>
  );
};

export default Compose;
