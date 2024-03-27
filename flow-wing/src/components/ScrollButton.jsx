import React, { useState, useEffect } from "react"
import "./scrollButton.css"
import { Icon } from "@iconify/react"

const ScrollButton = () => {
  const [visible, setVisible] = useState(false)

  useEffect(() => {
    const toggleVisible = () => {
      const scrolled = document.documentElement.scrollTop
      if (scrolled > 300) {
        setVisible(true)
      } else if (scrolled <= 300) {
        setVisible(false)
      }
    }

    window.addEventListener("scroll", toggleVisible)

    // Clean-up
    return () => {
      window.removeEventListener("scroll", toggleVisible)
    }
  }, []) // Run only once when component mounts

  const scrollToTop = () => {
    window.scrollTo({
      top: 0,
      behavior: "smooth"
      /* you can also use 'auto' behaviour 
         in place of 'smooth' */
    })
  }

  return (
    <button
      style={{ display: visible ? "inline" : "none" }}
      onClick={scrollToTop}
    >
      <Icon
        icon="fe:arrow-up"
        width="46"
        height="46"
        style={{ color: "#34c38f " }}
      />
    </button>
  )
}

export default ScrollButton
