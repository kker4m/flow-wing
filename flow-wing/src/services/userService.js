import apiAxios from "../lib/apiAxios"

const getUsers = () => {
  return apiAxios.get("Users")
}

export { getUsers }
