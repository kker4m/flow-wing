import React from "react";
import data from "../../data.json";
import "./inbox.css";
import { Icon } from "@iconify/react";
import { Tooltip } from "@mui/material";
import { Divider } from "antd";

const Inbox = ({ index }) => {
  // Find the data item with the given id
  const selectedData = data.find((item) => item.index === index);

  if (!selectedData) {
    return <div>Data not found for id: {index}</div>;
  }

  return (
    <div className="inbox-page-content">
      <div className="mail-actions">
        <Tooltip title="İlet" arrow>
          {" "}
          <div className="icons">
            <button className="mail-action-btns">
              {" "}
              <Icon
                icon="material-symbols-light:forward"
                width="40"
                rotate={2}
              />
            </button>
          </div>
        </Tooltip>
        <Tooltip title="Yanıtla" arrow>
          {" "}
          <div className="icons">
            <button className="mail-action-btns">
              <Icon icon="iconoir:reply" width="30" />
            </button>
          </div>{" "}
        </Tooltip>
        <Tooltip title="Sil" arrow>
          <div className="icons">
            <button className="mail-action-btns">
              {" "}
              <Icon icon="bi:trash" width="30" />
            </button>
          </div>
        </Tooltip>
      </div>
      <div className="mail-sender">
        <div className="user-icon-home">
          <Icon icon="ph:user-light" width="70" />{" "}
        </div>
        <div>
          <div className="icon-with-text">
            <Icon icon="bi:ticket-fill" color="#b31312" width="30" />
            <span>Tuğçe</span>
          </div>

          <div className="mail-sender-email"> {selectedData.sender}</div>
        </div>
      </div>

      <div className="mail-title">
        <Icon icon="uit:subject" color="#b31312" width="40" />{" "}
        <h3>{selectedData.title}</h3>
      </div>
      <p>{selectedData.body}</p>

      <div className="mail-attachments">
        <div className="attachment-content">
          <Icon
            icon="material-symbols-light:attachment"
            color="#b31312"
            width="30"
          />
          <span>{selectedData.attachment}</span>
        </div>
        <Divider />
      </div>

      <div className="inbox-mail-summary">
        <p>
          <span>Kimden:</span> {selectedData.sender}
        </p>
        <p>
          <span>Konu:</span> {selectedData.title}
        </p>
        <p>
          <span>Tarih:</span> {selectedData.sentTime}
        </p>
      </div>
    </div>
  );
};

export default Inbox;
