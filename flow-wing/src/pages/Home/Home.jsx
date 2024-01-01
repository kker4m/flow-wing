import React from "react";
import "./home.css";
import data from "../../data.json";
import { Icon } from "@iconify/react";
import { Link } from "react-router-dom";
import { Divider, Input } from "antd";
import Header from "../../components/Header";

const Home = ({ index }) => {
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
        {data.map((item, index) =>
          item.isOpened ? (
            <Link to={`/inbox/${index}`} key={index}>
              <div key={index} className="inbox-mail-unopened">
                <div className="inbox-mail-sender">
                  <div className="mail-primary-icon">
                    {" "}
                    <Icon icon="icon-park-solid:right-c" color="#f0de36" />
                  </div>

                  {item.sender}
                </div>
                <div className="inbox-mail-body">
                  {excerpt(item.body, 120)}

                 
                
                </div>
                <div className="inbox-sent-time">{item.sentTime}</div>
              </div><Divider/>
            </Link>
          ) : (
            <Link to={`/inbox/${index}`} key={index}>
              <div key={index} className="inbox-mail-opened">
                <div className="inbox-mail-sender">
                  <div className="mail-primary-icon">
                    {" "}
                    <Icon icon="icon-park-solid:right-c" color="#f0de36" />
                  </div>

                  {item.sender}
                </div>
                <div className="inbox-mail-body">
                  {excerpt(item.body, 120)}
                </div>
                <div className="inbox-sent-time">{item.sentTime}</div>
              </div>  <Divider/>
            </Link>
          )
        )}
     
      </div>
    </div>
  );
};

export default Home;
