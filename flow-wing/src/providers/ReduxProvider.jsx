import { Provider } from "react-redux"
import store from "../Redux/Store/store"

function ReduxProvider({ children }) {
  return <Provider store={store}>{children}</Provider>
}

export default ReduxProvider
