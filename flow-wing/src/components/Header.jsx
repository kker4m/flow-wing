import React, { useState } from 'react'

const getUser = ()=>{
  let user = localStorage.getItem("user");
  if (user){
    user = JSON.parse(user)
    console.log(user)
  }
  else {
    user = null
  }
  return user;
}

const Header = () => {
  const [user,setUser]= useState(getUser())
  return (
    <>
      {user? (
        <div>
          {user.fullname}
        </div>
      ):(<div>Kullanıcı yok</div>)}
    </>
  )
}

export default Header