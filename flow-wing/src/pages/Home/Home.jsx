import React from "react";
import "./home.css";
import data from "../../data.json";
import { Icon } from "@iconify/react";
import { Link } from "react-router-dom";
import { Input } from "antd";

const Home = ({ index }) => {

  // to shorten the mail body 
  const excerpt = (str, count) => {
    if (str && str.length > count) {
      str = str.substring(0, count) + "...";
    }
    return str;
  };

  const { Search } = Input;

const onSearch = (value, _e, info) => console.log(info?.source, value);

  return (
    <div className="home-page-content">
    <div className="search-section">
    <Search
      placeholder="Postalarda arayın"
      onSearch={onSearch}
      style={{
        width: 200,
      }}
    />
    </div>
      <div className="inbox">
        {data.map((item, index) => (
          item.isOpened ?(<Link to={`/inbox/${index}`} key={index}>
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

                {item.attachment && (
                  <div className="mail-attachment">      {excerpt(item.attachment, 10)}</div>
                )}
              </div>
              <div className="inbox-sent-time">
              {item.sentTime}
              </div>
            </div>
          </Link>):(
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

                {item.attachment && (
                  <div className="mail-attachment"> {item.attachment}</div>
                )}
              </div>
              <div className="inbox-sent-time">
              {item.sentTime}
              </div>
            </div>
          </Link> 
          )
          
        ))}
      </div>
    </div>
  );
};

export default Home;
