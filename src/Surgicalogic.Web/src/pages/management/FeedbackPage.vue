<template>
  <div class="container fluid grid-list-md">
    <grid-component :headers="headers"
                    :items="feedbacks"
                    :title="title"
                    :show-detail="true"
                    :show-edit="false"
                    :show-delete="true"
                    :show-search="true"
                    :show-insert="false"
                    :loading="getLoading"
                    :methodName="getMethodName"
                    :totalCount="getTotalCount"
                    @detail="detail"
                    @deleteitem="deleteItem"
                    @exportToExcel="exportFeedbackToExcel">
    </grid-component>

    <feedback-detail-component :detail-action="detailAction"
                               :detail-visible="detailDialog"
                               @cancel="cancel">
    </feedback-detail-component>

    <delete-component :delete-value="deleteValue"
                      :delete-visible="deleteDialog"
                      :deleteMethode="deleteMethodName"
                      @cancel="cancel">
    </delete-component>
  </div>
</template>

<script>

import { gridMixin } from './../../mixins/gridMixin';

export default {
  mixins: [
    gridMixin
  ],

  data() {
    const vm = this;

    return {
      search: '',
      detailDialog: false,
      detailAction: {},
      totalRowCount:0,
      deleteDialog: false,
      deleteValue: {},
      deletePath: 'deleteFeedback'
    };
  },

  computed: {
    title() {
      const vm = this;

      return vm.$i18n.t('feedbacks.feedbacks');
    },

    headers() {
      const vm = this;

      //Columns and actions
      return [
        {
          value: 'email',
          text: vm.$i18n.t('feedbacks.email'),
          sortable: true,
          align: 'left'
        },
        {
          value: 'body',
          text: vm.$i18n.t('feedbacks.body'),
          sortable: true,
          align: 'left'
        },
        {
          isAction: true,
          sortable: false,
          align: 'right'
        }
      ];
    },

    feedbacks() {
      const vm = this;

      return vm.$store.state.feedbackModule.feedbacks;
    },

    getLoading() {
      const vm = this;

      return vm.$store.state.feedbackModule.loading;
    },

    getTotalCount() {
      const vm = this;

      return vm.$store.state.feedbackModule.totalCount;
    }
  },

  methods: {
    getMethodName(){
      return "getFeedbacks";
    },

    deleteMethodName(){
      return "deleteFeedback";
    },

    exportFeedbackToExcel() {
      const vm = this;

      vm.$store.dispatch('excelExportFeedbacks');
    }
  }
};

</script>
