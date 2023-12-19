import React from "react";
import data from "../../data.json";
import "./inbox.css";

const Inbox = ({ index }) => {
  // Find the data item with the given id
  const selectedData = data.find((item) => item.index === index);

  if (!selectedData) {
    return <div>Data not found for id: {index}</div>;
  }

  return (
    <div className="inbox-page-content">
      <div className="inbox-mail-title">
        <h3>{selectedData.title}</h3>
      </div>
      <p> {selectedData.sender}</p>
      <p>{selectedData.body}</p>
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
