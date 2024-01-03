import React from "react";
import "./home.css";
import data from "../../data.json";
import { Icon } from "@iconify/react";
import { Link } from "react-router-dom";
import Divider from "@mui/material/Divider";

const Home = () => {
  // to shorten the mail body
  const excerpt = (str, count) => {
    if (str && str.length > count) {
      str = str.substring(0, count) + "...";
    }
    return str;
  };

  return (
    <div className="home-page-content">
      <div className="inbox">
        {data.map((item, index) => (
          <Link to={`/inbox/${index}`} key={index}>
            <div className="mail-content">
              <div key={index} className="inbox-mail-unopened">
                <div className="user-section">
                  <div className="user-icon-home">
                    <Icon icon="ph:user-light" width="30" />{" "}
                  </div>

                  <div className="user-name">{item.sender} </div>
                </div>
                <div className="inbox-mail-body">{excerpt(item.body, 120)}</div>
              </div>{" "}
              <div className="inbox-sent-time">{item.sentTime}</div>
            </div>{" "}
            <Divider />
          </Link>
        ))}
      </div>
    </div>
  );
};

export default Home;
