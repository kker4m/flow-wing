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
  };
  const onSelect = (option) => {
    console.log("select", option);
  };

  // FORMIK
  const formik = useFormik({
    initialValues: {
      recipientsEmail: "",
      emailSubject: "",
      emailBody: "",
    },
    onSubmit: (values) => {
      handleSubmit(values);
    },
  });

  return (
    <div className="compose-page-content">
      <h2>Mail Oluştur</h2>

      <div className="compose send-to">
        <span>Kime</span> <Divider />
        <Mentions
        style={{height:50, border:"none"}}
          onChange={(value) => {
            // value contains the current input value
            formik.handleChange({ target: { name: "recipientsEmail", value } });
          }}
          onSelect={(value) => {
            // option contains the selected value
            formik.setFieldValue("recipientsEmail", value);
          }}
          value={formik.values.recipientsEmail}
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
      <div className="compose mail-body"><span>İçerik</span> <Divider />
        <textarea
          name="emailBody"
          value={formik.values.emailBody}
          onChange={formik.handleChange}
        />
      </div>
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
