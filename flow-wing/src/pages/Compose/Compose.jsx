import React from "react";
import Attachments from "../../components/Attachments";
import { Input } from "antd";
import TextArea from "antd/es/input/TextArea";
import "./compose.css";

const Compose = () => {
  return (
    <div className="compose-page-content">
      <div className="compose sender">
        <label>Gönderen: </label>
        <label>tugceozelmaci1@gmail.com</label>
      </div>
      <div className="compose send-to">
        <label>Kime:</label>
        <Input />
      </div>
      <div className="compose mail-body">
        <label>Mail:</label>
        <TextArea />
      </div>
      <div className="compose-attachments">
        <Attachments />
      </div>
      <button>Gönder</button>
    </div>
  );
};

export default Compose;
