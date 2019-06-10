export const localizationMixin = {
  methods: {
    getLocale()
    {
        const vm = this;

        return vm.$cookie.get("currentLanguage") == "tr" ? "tr-TR" : "en-US";
    }
  }
}
