import React, { useState } from "react";
import Attachments from "../../components/Attachments";
import { Input } from "antd";
import TextArea from "antd/es/input/TextArea";
import ReactQuill from "react-quill";
import "./compose.css";

const Compose = () => {
  const modules = {
    toolbar: [
      ["bold", "italic", "underline", "strike", "blockquote"],
      [{ size: [] }],
      [{ font: [] }],
      [{ align: ["right", "center", "justify"] }],
      [{ list: "ordered" }, { list: "bullet" }],
      ["link", "image"],
      [{ color: ["red", "#785412"] }],
      [{ background: ["red", "#785412"] }],
    ],
  };

  const formats = [
    "header",
    "bold",
    "italic",
    "underline",
    "strike",
    "blockquote",
    "list",
    "bullet",
    "link",
    "color",
    "image",
    "background",
    "align",
    "size",
    "font",
  ];
  const [value, setValue] = useState("");
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
        <ReactQuill
          theme="snow"
          modules={modules}
          formats={formats}
          value={value}
          style={{ height: "400px", border: "none" }}
        />
      </div>
      <div className="compose-attachments">
        <Attachments />
      </div>
      <div className="compose-btns">
        <button className="send-btn">Gönder</button>
        <button className="delete-btn">Sil</button>
      </div>
    </div>
  );
};

export default Compose;
