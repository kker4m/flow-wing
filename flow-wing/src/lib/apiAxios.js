import axios from "axios"

// Create an instance of Axios
const apiAxios = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  mode: "cors"
})
apiAxios.interceptors.request.use(
  function (config) {
    const userData = localStorage.getItem("user")
    const userObject = JSON.parse(userData)
    const userToken = userObject.token
    config.headers = {
      ...config.headers,
      Authorization: `Bearer ${userToken}`,
      "Content-Type": "multipart/form-data"
    }

    // Do something before request is sent
    // console.log("Request Interceptor - Request Config: ", config)
    return config
  },
  function (error) {
    // Do something with request error
    return Promise.reject(error)
  }
)

// Add a response interceptor
apiAxios.interceptors.response.use(
  function (response) {
    // Do something with response data
    //console.log("Response Interceptor - Response Data: ", response.data)
    return response
  },
  function (error) {
    // Do something with response error
    return Promise.reject(error)
  }
)
export default apiAxios
