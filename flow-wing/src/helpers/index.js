export const excerpt = (str, count) => {
  if (str && str.length > count) {
    str = str.substring(0, count) + "..."
  }
  return str
}
export const getText = (html) => {
  try {
    const doc = new DOMParser().parseFromString(html, "text/html")
    return doc.body.innerHTML
  } catch (error) {
    console.error("Hata oluştu:", error)
    return null
  }
}
