import React, { useEffect, useState } from "react"
import Attachments from "../../components/Attachments"
import { Divider, Icon, MenuItem } from "@mui/material"
import { useSelector } from "react-redux"
import { useFormik } from "formik"
import "./compose.css"
import { useNavigate } from "react-router"
import alertify from "alertifyjs"
import { Checkbox, DatePicker, Mentions, Modal, Select, Upload } from "antd"
import * as Yup from "yup"
import dayjs from "dayjs"
import {
  sendMail,
  sendScheduledMail,
  sendScheduledRepeatingMail
} from "../../services/emailService"
import { getUsers } from "../../services/userService"

        
const Compose = () => {
  const [users, setUsers] = useState([])
  const [isModalOpen, setIsModalOpen] = useState(false)
  const [isRepeating, setIsRepeating] = useState(false)
  const [isScheduled, setIsScheduled] = useState(false)
  // MODAL FUNCTIONS
  const showModal = () => {
    setIsModalOpen(true)
  }
  const handleOk = () => {
    setIsModalOpen(false)
  }
  const handleCancel = () => {
    setIsModalOpen(false)
  }
  const navigate = useNavigate()
  // GET USER
  const user = useSelector((state) => state.user.user)

  // MAIL SEND FUNCTION FOR NON REPEATING MAIL
  const handleSubmit = async (values) => {
    try {
      const formData = new FormData()
      if (values.file && values.file.length > 0) {
        formData.append("attachment", values.file[0])
      } else {
        formData == []
      }
      console.log("values:", values, "form data ", formData)
      const res = await sendMail(values, formData)
      if (res.status === 201) {
        alertify.success("Mail Gönderildi")
        navigate("/home")
      } else {
        alertify.error("Gönderme başarısız oldu")
      }
    } catch (error) {
      console.error("Error submitting form:", error)
      alertify.error("Bir hata oluştu")
    }
  }

  // MAIL SEND FUNCTION FOR REPEATING MAIL

  const handleSubmitRepeating = (values) => {
    sendScheduledRepeatingMail(values).then((res) => {
      if (res.status === 201) {
        alertify.success("Mail Gönderildi")
        navigate("/home")
      } else alertify.error("Gönderme başarısız oldu")
    })
  }
  // MAIL SEND FUNCTION FOR SCHEDULED MAIL

  const handleSubmitScheduled = (values) => {
    sendScheduledMail(values).then((res) => {
      if (res.status === 201) {
        alertify.success("Mail şu tarih için gönderilecek: ")
        navigate("/home")
      } else alertify.error("Gönderme başarısız oldu")
    })
  }

  // ANTD MENTION FUNCTIONS
  const onMentionChange = (value) => {
    console.log("Change:", value)
    formik.handleChange({ target: { name: "recipientsEmail", value } })
  }

  const onMentionSelect = (option) => {
    console.log("select", option)
    formik.setFieldValue("recipientsEmail", option.value)
  }

  // DATE FIELDS

  const onDateSelect = (value) => {
    formik.setFieldValue("nextSendingDate", dayjs(value))
    console.log("next sending date seçildi", value)
  }

  const onRepeatIntervalDateSelect = (value) => {
    // Burada seçilen değeri formik formunun değerlerine ekleyin
    formik.setFieldValue("repeatInterval", value)
    console.log("repeat interval date seçildi", value)
  }

  const onRepeatEndDateSelect = (value) => {
    formik.setFieldValue("repeatEndDate", dayjs(value))
    console.log("repeat ending date seçildi", value)
  }

  const onSendDateTimeSelect = (value) => {
    setIsScheduled(true)
    formik.setFieldValue("sentDateTime", dayjs(value))
    console.log("schedule send date seçildi", value)
  }
  //Yup validation schema
  const validationSchema = Yup.object().shape({
    recipientsEmail: Yup.string()
      .email("Geçersiz mail adresi")
      .required("Mail adresi girmek zorunludur"),
    emailSubject: Yup.string().required("Konu giriniz"),
    emailBody: Yup.string().required("İçerik giriniz")
  })

  // FORMIK

  const initialValues = {
    recipientsEmail: "",
    emailSubject: "",
    emailBody: "",
    nextSendingDate: "",
    repeatInterval: "",
    repeatEndDate: "",
    sentDateTime: "",
    file: []
  }

  const formik = useFormik({
    initialValues,
    validationSchema: validationSchema,
    onSubmit: (values) => {
      const { repeatEndDate, repeatInterval, nextSendingDate, sentDateTime } =
        values
      if (isScheduled) {
        handleSubmitScheduled({ ...values, sentDateTime }) // Eğer isScheduled true ise, planlanmış gönderimi gerçekleştir
      } else if (isRepeating) {
        // Eğer isRepeating true ise, ekstra değerler ile birlikte gönder
        handleSubmitRepeating({
          ...values,
          nextSendingDate,
          repeatInterval,
          repeatEndDate
        })
      } else {
        // Değilse, sadece form alanlarını gönder
        handleSubmit(values)
      }
    }
  })
  // GET USERS FOR MENTION
  useEffect(() => {
    getUsers()
      .then((res) => {
        setUsers(res.data)
      })
      .catch((error) => {
        console.error("Error fetching users:", error)
      })

    return () => {}
  }, [])

  const options = users.map((user) => ({
    value: user.email,
    label: user.email
  }))
  const handleReset = () => {
    formik.resetForm() // Reset the form fields
  }
  // CHECKBOX
  const handleCheck = (event) => {
    if (event.target.checked) {
      console.log("Checkbox işaretlendi")
      setIsRepeating(true)
      showModal()
    }
  }
  // ATTACHMENT FUNCTION
  const handleFileChange = (event) => {
    if (event.target.files.length > 0) {
      console.log("Selected files:", event.target.files)
      // Dosya nesnesini al
      const file = event.target.files[0]
      // Form verilerine dosya nesnesini ekle
      formik.setFieldValue("attachment", file)
    } else {
      console.log("No file selected")
      formik.setFieldValue("attachment", [])
    }
  }

  return (
    <div className="compose-page-content">
      <form onSubmit={formik.handleSubmit}>
        <h2>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="56"
            height="56"
            viewBox="0 0 28 28"
          >
            <path
              fill="black"
              d="M25.707 3.707a1 1 0 0 0-1.414-1.414l-13 13L11 17l1.707-.293zM6.5 3A3.5 3.5 0 0 0 3 6.5v15A3.5 3.5 0 0 0 6.5 25h15a3.5 3.5 0 0 0 3.5-3.5v-10a1 1 0 1 0-2 0v10a1.5 1.5 0 0 1-1.5 1.5h-15A1.5 1.5 0 0 1 5 21.5v-15A1.5 1.5 0 0 1 6.5 5h10a1 1 0 1 0 0-2z"
            />
          </svg>
        </h2>
        <div className="checkbox">
          <Checkbox onChange={handleCheck} style={{ width: 120 }}>
            Tekrarla
          </Checkbox>
          <br />
          <div>
            Şu tarihte gönder:
            <br />
            <DatePicker
              onSelect={onSendDateTimeSelect}
              value={formik.values.sentDateTime}
            />
          </div>
        </div>

        {/* MODAL FOR DATE PICKING TO SEND SCHEDULED MAIL */}
        <Modal
          title="Basic Modal"
          open={isModalOpen}
          onOk={handleOk}
          onCancel={handleCancel}
        >
          <h2>Ne zaman gönderilsin?</h2>
          <DatePicker
            onSelect={onDateSelect}
            value={formik.values.nextSendingDate}
          />
          <h2>Ne sıklıkla gönderilsin?</h2>

          <Select
            style={{ width: 120 }}
            defaultValue="30-00-00-00"
            value={formik.values.repeatInterval}
            onChange={onRepeatIntervalDateSelect}
          >
            <MenuItem value={"00-07-00-00"}>Haftada bir</MenuItem>
            <MenuItem value={"30-00-00-00"}>Ayda bir</MenuItem>
            <MenuItem value={"00-00-00-60"}>Dakikada bir</MenuItem>
          </Select>
          <h2>Ne zamana kadar gönderilsin?</h2>
          <DatePicker
            onSelect={onRepeatEndDateSelect}
            value={formik.values.repeatEndDate}
          />
        </Modal>
        <div className="compose send-to">
          <span>Kime</span> <Divider />
          <Mentions
            allowClear
            style={{ height: 50, border: "none" }}
            onChange={onMentionChange}
            onSelect={onMentionSelect}
            value={formik.values.recipientsEmail}
            required
            options={options}
          />
        </div>
        {formik.errors.recipientsEmail && formik.touched.recipientsEmail && (
          <div className="error-message">{formik.errors.recipientsEmail}</div>
        )}
        <div className="compose send-to">
          <span>Konu</span>
          <Divider />
          <input
            type="text"
            name="emailSubject"
            onChange={formik.handleChange}
            value={formik.values.emailSubject}
          ></input>
        </div>
        {formik.errors.emailSubject && formik.touched.emailSubject && (
          <div className="error-message">{formik.errors.emailSubject}</div>
        )}
        <div className="compose mail-body">
          <span>İçerik</span> <Divider />
        
          <textarea
            name="emailBody"
            value={formik.values.emailBody}
            onChange={formik.handleChange}
          />
        </div>
        {formik.errors.emailBody && formik.touched.emailBody && (
          <div className="error-message">{formik.errors.emailBody}</div>
        )}
        <div className="compose-attachments">
          <input
            id="file"
            name="file"
            type="file"
            onChange={(event) => {
              formik.setFieldValue("file", event.currentTarget.files[0])
            }}
          />

          <hr />
        </div>
        <div className="compose-btns">
          <button className="send-btn" type="submit">
            Gönder
          </button>
          <button className="delete-btn" onClick={handleReset}>
            Sil
          </button>
        </div>
      </form>
    </div>
  )
}

export default Compose
