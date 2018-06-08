/* ============
 * Locale
 * ============
 *
 * For a multi-language application,
 * specify the languages you want to use here.
 */
/*
USAGE
  {{ $t("message.hello") }} in page
  i18n.t("equipments.name") in js
*/

import en from './en';
import tr from './tr';

export default {
  en,
  tr,
};